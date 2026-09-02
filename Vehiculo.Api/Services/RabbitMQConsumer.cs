using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using Vehiculo.Api.Data;
using Vehiculo.Api.Models;

namespace Vehiculo.Api.Services
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;

        public RabbitMQConsumer(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "rabbitmq",
                Port = int.TryParse(_configuration["RabbitMQ:Port"], out var p) ? p : 5672,
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();
            var queueName = _configuration["RabbitMQ:QueueName"] ?? "CategoriasCreadasQueue";

            await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensajeJson = Encoding.UTF8.GetString(body);
                Console.WriteLine($"[RabbitMQ-Vehiculo] Mensaje Recibido: {mensajeJson}");

                try
                {
                    // Deserializamos asumiendo el formato de la categoría
                    using var jsonDoc = JsonDocument.Parse(mensajeJson);
                    var idElement = jsonDoc.RootElement.GetProperty("Idcategoria");
                    var idCategoria = idElement.ValueKind == JsonValueKind.Number ? idElement.GetInt32() : int.Parse(idElement.GetString()!);
                    var nombre = jsonDoc.RootElement.GetProperty("Nombre").GetString();

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<VehiculoDbContext>();

                    // Idempotencia: Verificar si ya existe en caché
                    var existe = dbContext.CategoriaCache.Any(c => c.Idcategoria == idCategoria);
                    
                    if (!existe)
                    {
                        dbContext.CategoriaCache.Add(new CategoriaCacheEntidad 
                        { 
                            Idcategoria = idCategoria, 
                            Nombre = nombre 
                        });
                        await dbContext.SaveChangesAsync();
                        Console.WriteLine($"[RabbitMQ-Vehiculo] Categoría {idCategoria} ({nombre}) guardada en caché.");
                    }
                    else
                    {
                        Console.WriteLine($"[RabbitMQ-Vehiculo] La categoría {idCategoria} ya existía en caché. Se ignoró por idempotencia.");
                    }

                    // Acknowledge del mensaje (se procesó bien)
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[RabbitMQ-Vehiculo] Error procesando caché: {ex.Message}");
                    // Nack: Algo salió mal con la BD, reencolamos el mensaje para no perderlo
                    await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            // autoAck = false porque usamos acuses de recibo manuales arriba (BasicAck / BasicNack)
            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}


using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Categoria.Api.Models;

namespace Categoria.Api.Services
{
    public class RabbitMQPublisher
    {
        private readonly IConfiguration _configuration;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PublicarCategoriaCreadaAsync(CategoriaEntidad categoria)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"] ?? "rabbitmq",
                Port = int.TryParse(_configuration["RabbitMQ:Port"], out var p) ? p : 5672,
                UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                Password = _configuration["RabbitMQ:Password"] ?? "guest"
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var queueName = _configuration["RabbitMQ:QueueName"] ?? "CategoriasCreadasQueue";

            await channel.QueueDeclareAsync(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var json = JsonSerializer.Serialize(categoria);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(exchange: string.Empty,
                                 routingKey: queueName,
                                 mandatory: true,
                                 basicProperties: properties,
                                 body: body);
        }
    }
}

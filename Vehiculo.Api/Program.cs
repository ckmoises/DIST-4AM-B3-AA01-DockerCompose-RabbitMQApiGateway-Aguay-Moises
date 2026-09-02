using Microsoft.EntityFrameworkCore;
using Vehiculo.Api.Data;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHostedService<Vehiculo.Api.Services.RabbitMQConsumer>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<VehiculoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibrosConnection")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();



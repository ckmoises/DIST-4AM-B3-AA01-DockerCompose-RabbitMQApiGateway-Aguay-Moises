var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/categoria/v1/swagger.json", "API Categorias");
    c.SwaggerEndpoint("/swagger/vehiculo/v1/swagger.json", "API Vehiculos");
});

app.MapReverseProxy();

app.Run();

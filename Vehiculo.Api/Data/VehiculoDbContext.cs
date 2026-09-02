using Microsoft.EntityFrameworkCore;
using Vehiculo.Api.Models;

namespace Vehiculo.Api.Data
{
    public class VehiculoDbContext : DbContext
    {
        public VehiculoDbContext(DbContextOptions<VehiculoDbContext> options) : base(options)
        {
        }

        public DbSet<VehiculoEntidad> Vehiculos { get; set; }
        public DbSet<CategoriaCacheEntidad> CategoriaCache { get; set; }
    }
}


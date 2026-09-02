using Categoria.Api.Models;
using Microsoft.EntityFrameworkCore;
using Categoria.Api.Models;

namespace Categoria.Api.Data
{
    public class CategoriaDbContext : DbContext
    {
        public CategoriaDbContext(DbContextOptions<CategoriaDbContext> options) : base(options)
        {
        }

        public DbSet<CategoriaEntidad> Categorias { get; set; }
        
    }
}



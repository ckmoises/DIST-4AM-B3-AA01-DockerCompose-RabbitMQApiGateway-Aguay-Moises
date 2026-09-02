using Categoria.Api.Data;
using Categoria.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Categoria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaDbContext _context;

        private readonly Categoria.Api.Services.RabbitMQPublisher _rabbitPublisher;

        public CategoriasController(CategoriaDbContext context, Categoria.Api.Services.RabbitMQPublisher rabbitPublisher)
        {
            _context = context;
            _rabbitPublisher = rabbitPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaEntidad>>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaEntidad>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            return categoria;
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaEntidad>> PostCategoria(CategoriaEntidad categoria)
        {
            categoria.Idcategoria = 0; // Para Identity
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            await _rabbitPublisher.PublicarCategoriaCreadaAsync(categoria);

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.Idcategoria }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaEntidad categoria)
        {
            if (id != categoria.Idcategoria)
            {
                return BadRequest();
            }

            _context.Entry(categoria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Idcategoria == id);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehiculo.Api.Data;
using Vehiculo.Api.Models;
namespace Vehiculo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiculosController : ControllerBase
    {
        private readonly VehiculoDbContext _context;
        

        public VehiculosController(VehiculoDbContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoEntidad>>> GetVehiculos()
        {
            return await _context.Vehiculos.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoEntidad>> GetVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return vehiculo;
        }

        [HttpPost]
        public async Task<ActionResult<VehiculoEntidad>> PostVehiculo(VehiculoEntidad vehiculo)
        {
            // Validar existencia de categoría en Caché
            var categoriaExiste = await _context.CategoriaCache.AnyAsync(c => c.Idcategoria == vehiculo.idcategoria);
            if (!categoriaExiste)
            {
                return BadRequest($"La categoría con ID {vehiculo.idcategoria} no existe en caché. No se puede crear el vehículo.");
            }

            vehiculo.IdVehiculo = 0; // Para Identity
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.IdVehiculo }, vehiculo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVehiculo(int id, VehiculoEntidad vehiculo)
        {
            if (id != vehiculo.IdVehiculo)
            {
                return BadRequest();
            }

            _context.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VehiculoExists(id))
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
        public async Task<IActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
            {
                return NotFound();
            }

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VehiculoExists(int id)
        {
            return _context.Vehiculos.Any(e => e.IdVehiculo == id);
        }
    }
}



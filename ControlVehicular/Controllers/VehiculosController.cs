using ControlVehicular.Data;
using ControlVehicular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicular.Controllers
{
	[Route("api/[controller]")]
	[Route("api/vehiculo")]
	[ApiController]
	public class VehiculosController : ControllerBase
	{
		private readonly ApplicationDbContext dbContext;

		public VehiculosController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Vehiculo>>> ObtenerVehiculos()
		{
			var vehiculos = await dbContext.Vehiculos
				.Include(v => v.CatMarca)
				.AsNoTracking()
				.ToListAsync();

			return Ok(vehiculos);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Vehiculo>> ObtenerVehiculoPorId(int id)
		{
			var vehiculo = await dbContext.Vehiculos
				.Include(v => v.CatMarca)
				.AsNoTracking()
				.FirstOrDefaultAsync(v => v.Id == id);

			if (vehiculo == null)
			{
				return NotFound(new { mensaje = $"No se encontró el vehículo con Id {id}." });
			}

			return Ok(vehiculo);
		}

		[HttpPost]
		public async Task<ActionResult<Vehiculo>> CrearVehiculo([FromBody] Vehiculo vehiculo)
		{
			if (vehiculo == null)
			{
				return BadRequest(new { mensaje = "Los datos del vehículo son requeridos." });
			}

			if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
			{
				return BadRequest(new { mensaje = "El modelo del vehículo es requerido." });
			}

			if (string.IsNullOrWhiteSpace(vehiculo.Year) || vehiculo.Year.Length != 4)
			{
				return BadRequest(new { mensaje = "El año del vehículo debe contener exactamente 4 dígitos." });
			}

			var marcaExiste = await dbContext.CatMarcas.AnyAsync(m => m.Id == vehiculo.CatMarcaId);
			if (!marcaExiste)
			{
				return BadRequest(new { mensaje = $"No existe una marca registrada con el Id {vehiculo.CatMarcaId}." });
			}

			vehiculo.Id = 0;
			vehiculo.CatMarca = null;

			dbContext.Vehiculos.Add(vehiculo);
			await dbContext.SaveChangesAsync();

			await dbContext.Entry(vehiculo).Reference(v => v.CatMarca).LoadAsync();

			return Ok(vehiculo);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> ActualizarVehiculo(int id, [FromBody] Vehiculo vehiculo)
		{
			if (vehiculo == null)
			{
				return BadRequest(new { mensaje = "Los datos del vehículo son requeridos." });
			}

			if (string.IsNullOrWhiteSpace(vehiculo.Modelo))
			{
				return BadRequest(new { mensaje = "El modelo del vehículo es requerido." });
			}

			if (string.IsNullOrWhiteSpace(vehiculo.Year) || vehiculo.Year.Length != 4)
			{
				return BadRequest(new { mensaje = "El año del vehículo debe contener exactamente 4 dígitos." });
			}

			var vehiculoExistente = await dbContext.Vehiculos.FindAsync(id);
			if (vehiculoExistente == null)
			{
				return NotFound(new { mensaje = $"No se encontró el vehículo con Id {id}." });
			}

			var marcaExiste = await dbContext.CatMarcas.AnyAsync(m => m.Id == vehiculo.CatMarcaId);
			if (!marcaExiste)
			{
				return BadRequest(new { mensaje = $"No existe una marca registrada con el Id {vehiculo.CatMarcaId}." });
			}

			vehiculoExistente.Modelo = vehiculo.Modelo.Trim();
			vehiculoExistente.Year = vehiculo.Year.Trim();
			vehiculoExistente.CatMarcaId = vehiculo.CatMarcaId;

			await dbContext.SaveChangesAsync();

			await dbContext.Entry(vehiculoExistente).Reference(v => v.CatMarca).LoadAsync();

			return Ok(vehiculoExistente);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> EliminarVehiculo(int id)
		{
			var vehiculo = await dbContext.Vehiculos.FindAsync(id);
			if (vehiculo == null)
			{
				return NotFound(new { mensaje = $"No se encontró el vehículo con Id {id}." });
			}

			dbContext.Vehiculos.Remove(vehiculo);
			await dbContext.SaveChangesAsync();

			return NoContent();
		}
	}
}

using ControlVehicular.Data;
using ControlVehicular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicular.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CatalogosController : ControllerBase
	{
		private readonly ApplicationDbContext dbContext;

		public CatalogosController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		
		[HttpGet("marcas")]
		public async Task<ActionResult<IEnumerable<CatMarca>>> ObtenerMarcas()
		{
			var marcas = await dbContext.CatMarcas
				.AsNoTracking()
				.ToListAsync();

			return Ok(marcas);
		}

		
		[HttpGet("marcas/{id:int}")]
		public async Task<ActionResult<CatMarca>> ObtenerMarcaPorId(int id)
		{
			var marca = await dbContext.CatMarcas
				.AsNoTracking()
				.FirstOrDefaultAsync(m => m.Id == id);

			if (marca == null)
			{
				return NotFound(new { mensaje = $"No se encontró la marca con Id {id}." });
			}

			return Ok(marca);
		}

		
		[HttpPost("marcas")]
		[HttpPost]
		public async Task<ActionResult<CatMarca>> CrearMarca([FromBody] CatMarca marca)
		{
			if (marca == null || string.IsNullOrWhiteSpace(marca.Marca))
			{
				return BadRequest(new { mensaje = "El nombre de la marca es requerido." });
			}

			
			marca.Id = 0;
			marca.Vehiculos = null;

			dbContext.CatMarcas.Add(marca);
			await dbContext.SaveChangesAsync();

			return Ok(marca);
		}

		[HttpPut("marcas/{id:int}")]
		public async Task<IActionResult> ActualizarMarca(int id, [FromBody] CatMarca marca)
		{
			if (marca == null || string.IsNullOrWhiteSpace(marca.Marca))
			{
				return BadRequest(new { mensaje = "El nombre de la marca es requerido." });
			}

			var marcaExistente = await dbContext.CatMarcas.FindAsync(id);
			if (marcaExistente == null)
			{
				return NotFound(new { mensaje = $"No se encontró la marca con Id {id}." });
			}

			marcaExistente.Marca = marca.Marca.Trim();
			await dbContext.SaveChangesAsync();

			return Ok(marcaExistente);
		}

		[HttpDelete("marcas/{id:int}")]
		public async Task<IActionResult> EliminarMarca(int id)
		{
			var marca = await dbContext.CatMarcas.FindAsync(id);
			if (marca == null)
			{
				return NotFound(new { mensaje = $"No se encontró la marca con Id {id}." });
			}

			bool tieneVehiculos = await dbContext.Vehiculos.AnyAsync(v => v.CatMarcaId == id);
			if (tieneVehiculos)
			{
				return BadRequest(new { mensaje = "No se puede eliminar la marca porque tiene vehículos asociados." });
			}

			dbContext.CatMarcas.Remove(marca);
			await dbContext.SaveChangesAsync();

			return NoContent();
		}
	}
}
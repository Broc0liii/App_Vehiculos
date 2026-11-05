using ControlVehicular.Data;
using ControlVehicular.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlVehicular.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ResponsablesController : ControllerBase
	{
		private readonly ApplicationDbContext dbContext;

		public ResponsablesController(ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Responsable>>> ObtenerResponsables()
		{
			var responsables = await dbContext.Responsables
				.AsNoTracking()
				.ToListAsync();

			return Ok(responsables);
		}

		[HttpGet("{id:int}")]
		public async Task<ActionResult<Responsable>> ObtenerResponsablePorId(int id)
		{
			var responsable = await dbContext.Responsables
				.AsNoTracking()
				.FirstOrDefaultAsync(r => r.Id == id);

			if (responsable == null)
			{
				return NotFound(new { mensaje = $"No se encontró el responsable con Id {id}." });
			}

			return Ok(responsable);
		}

		[HttpPost]
		public async Task<ActionResult<Responsable>> CrearResponsable([FromBody] Responsable responsable)
		{
			if (responsable == null)
			{
				return BadRequest(new { mensaje = "Los datos del responsable son requeridos." });
			}

			if (string.IsNullOrWhiteSpace(responsable.Nombre))
			{
				return BadRequest(new { mensaje = "El nombre es requerido." });
			}

			if (string.IsNullOrWhiteSpace(responsable.PrimerApellido))
			{
				return BadRequest(new { mensaje = "El primer apellido es requerido." });
			}

			if (string.IsNullOrWhiteSpace(responsable.Celular))
			{
				return BadRequest(new { mensaje = "El celular es requerido." });
			}

			responsable.Nombre = responsable.Nombre.Trim();
			responsable.PrimerApellido = responsable.PrimerApellido.Trim();
			responsable.SegundoApellido = string.IsNullOrWhiteSpace(responsable.SegundoApellido) ? null : responsable.SegundoApellido.Trim();
			responsable.Celular = responsable.Celular.Trim();

			dbContext.Responsables.Add(responsable);
			await dbContext.SaveChangesAsync();

			return Ok(responsable);
		}

		[HttpPut("{id:int}")]
		public async Task<IActionResult> ActualizarResponsable(int id, [FromBody] Responsable responsable)
		{
			if (responsable == null)
			{
				return BadRequest(new { mensaje = "Los datos del responsable son requeridos." });
			}

			if (string.IsNullOrWhiteSpace(responsable.Nombre))
			{
				return BadRequest(new { mensaje = "El nombre es requerido." });
			}

			if (string.IsNullOrWhiteSpace(responsable.PrimerApellido))
			{
				return BadRequest(new { mensaje = "El primer apellido es requerido." });
			}

			if (string.IsNullOrWhiteSpace(responsable.Celular))
			{
				return BadRequest(new { mensaje = "El celular es requerido." });
			}

			var responsableExistente = await dbContext.Responsables.FindAsync(id);
			if (responsableExistente == null)
			{
				return NotFound(new { mensaje = $"No se encontró el responsable con Id {id}." });
			}

			responsableExistente.Nombre = responsable.Nombre.Trim();
			responsableExistente.PrimerApellido = responsable.PrimerApellido.Trim();
			responsableExistente.SegundoApellido = string.IsNullOrWhiteSpace(responsable.SegundoApellido) ? null : responsable.SegundoApellido.Trim();
			responsableExistente.Celular = responsable.Celular.Trim();

			await dbContext.SaveChangesAsync();

			return Ok(responsableExistente);
		}

		[HttpDelete("{id:int}")]
		public async Task<IActionResult> EliminarResponsable(int id)
		{
			var responsable = await dbContext.Responsables.FindAsync(id);
			if (responsable == null)
			{
				return NotFound(new { mensaje = $"No se encontró el responsable con Id {id}." });
			}

			dbContext.Responsables.Remove(responsable);
			await dbContext.SaveChangesAsync();

			return NoContent();
		}
	}
}

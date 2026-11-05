using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ControlVehicular.Models
{
	public class Responsable
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es requerido.")]
		public string Nombre { get; set; } = string.Empty;

		[Required(ErrorMessage = "El primer apellido es requerido.")]
		public string PrimerApellido { get; set; } = string.Empty;

		public string? SegundoApellido { get; set; }

		[Required(ErrorMessage = "El celular es requerido.")]
		[JsonConverter(typeof(NumberOrStringConverter))]
		public string Celular { get; set; } = string.Empty;
	}
}

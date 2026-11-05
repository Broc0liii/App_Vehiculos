using System.ComponentModel.DataAnnotations;

namespace ControlVehicular.Models
{
	public class CatMarca
	{
		public int Id { get; set; }

		[Required]
		public string Marca { get; set; } = string.Empty;

		public ICollection<Vehiculo>? Vehiculos { get; set; } = new List<Vehiculo>();
	}
}

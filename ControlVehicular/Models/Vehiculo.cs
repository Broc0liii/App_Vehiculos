using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ControlVehicular.Models
{
	public class Vehiculo
	{
		public int Id { get; set; }

		[MinLength(4, ErrorMessage = "El campo Year debe contener 4 dígitos.")]
		[MaxLength(4, ErrorMessage = "El campo Year debe contener 4 dígitos.")]
		[JsonConverter(typeof(NumberOrStringConverter))]
		public string Year { get; set; } = string.Empty;

		[Required(ErrorMessage = "El modelo es requerido.")]
		public string Modelo { get; set; } = string.Empty;

		[Range(1, int.MaxValue, ErrorMessage = "Debe especificar un CatMarcaId válido.")]
		public int CatMarcaId { get; set; }

		public CatMarca? CatMarca { get; set; }
	}

	public class NumberOrStringConverter : JsonConverter<string>
	{
		public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType == JsonTokenType.Number)
			{
				if (reader.TryGetInt64(out long intVal))
				{
					return intVal.ToString();
				}
				return reader.GetDouble().ToString();
			}

			if (reader.TokenType == JsonTokenType.String)
			{
				return reader.GetString();
			}

			return null;
		}

		public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value);
		}
	}
}

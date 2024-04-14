using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class Forecast
	{
		[Required]
		public DateOnly Date { get; set; }

		[Required]
		[Range(-60, 60)]
		public sbyte MinTemperature { get; set; }

		[Required]
		[Range(-60, 60)]
		public sbyte MaxTemperature { get; set; }
	}
}

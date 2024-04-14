using System.ComponentModel.DataAnnotations;

namespace Infrastructure.EF.Entities
{
    public class ForecastEntity
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

using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ForecastView
    {
        [Required]
        public DateOnly Date { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;
    }
}

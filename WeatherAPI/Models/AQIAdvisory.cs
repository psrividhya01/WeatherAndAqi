using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class AQIAdvisory
    {
        [Key]
        public int AdvisoryId { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Advisory { get; set; } = string.Empty;
        public string SensitiveGroupNote { get; set; } = string.Empty;
    }
}

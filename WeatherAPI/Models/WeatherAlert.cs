using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class WeatherAlert
    {
        [Key]
        public int AlertId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

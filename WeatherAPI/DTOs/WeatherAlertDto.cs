using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class WeatherAlertDto
    {
        public string AlertId { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string AlertType { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}

namespace WeatherAPI.DTOs
{
    public class WeatherAlertDto
    {
        public string Event { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

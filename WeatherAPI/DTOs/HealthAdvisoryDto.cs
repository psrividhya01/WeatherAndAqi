namespace WeatherAPI.DTOs
{
    public class HealthAdvisoryDto
    {
        public string Category { get; set; } = string.Empty;
        public string Advisory { get; set; } = string.Empty;
        public string SensitiveGroupNote { get; set; } = string.Empty;
    }
}

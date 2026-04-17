namespace WeatherAPI.DTOs
{
    public class AQIRecommendationDto
    {
        public string Category { get; set; } = string.Empty;
        public string Advisory { get; set; } = string.Empty;
        public string SensitiveGroupNote { get; set; } = string.Empty;
    }
}

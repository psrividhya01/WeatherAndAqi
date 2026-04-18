namespace WeatherAPI.DTOs
{
    public class DailyDigestDto
    {
        public string City { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public double HighTemp { get; set; }
        public double LowTemp { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int AQIScore { get; set; }
        public string AQICategory { get; set; } = string.Empty;
        public string HealthTip { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }
}

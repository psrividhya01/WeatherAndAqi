namespace WeatherAPI.DTOs
{
    public class UserPreferencesDto
    {
        public string PreferredUnit { get; set; } = "metric";
        public bool ShowForecast { get; set; } = true;
        public bool ShowAQI { get; set; } = true;
        public bool ShowMap { get; set; } = true;
        public bool ShowHourlyChart { get; set; } = true;
        public bool ShowHealthAdvisory { get; set; } = true;
    }
}

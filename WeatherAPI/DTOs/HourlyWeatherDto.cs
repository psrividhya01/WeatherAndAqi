namespace WeatherAPI.DTOs
{
    public class HourlyWeatherDto
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public double PercipitationProbability { get; set; } = 0;
        public int ConditionCode { get; set; } = 0; // for theming (sunny, rain, etc)
        public string ConditionDescription { get; set; } = string.Empty; // e.g. "clear sky", "light rain"
    }
}

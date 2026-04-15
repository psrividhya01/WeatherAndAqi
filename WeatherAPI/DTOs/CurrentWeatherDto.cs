namespace WeatherAPI.DTOs
{
    public class CurrentWeatherDto
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public int ConditionCode { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

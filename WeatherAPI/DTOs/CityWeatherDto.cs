namespace WeatherAPI.DTOs
{
    public class CityWeatherDto
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int Aqi { get; set; }
        public string AqiCategory { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

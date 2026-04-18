namespace WeatherAPI.DTOs
{
    public class CityWeatherDto
    {
        public string CityName { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
    }
}

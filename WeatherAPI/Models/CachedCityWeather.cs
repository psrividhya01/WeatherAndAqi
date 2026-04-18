using System;

namespace WeatherAPI.Models
{
    public class CachedCityWeather
    {
        public int Id { get; set; }
        public string CityName { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public DateTime FetchedAt { get; set; }
    }
}

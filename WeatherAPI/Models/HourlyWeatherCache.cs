namespace WeatherAPI.Models
{
    public class HourlyWeatherCache
    {
        public int CacheId { get; set; }
        public string CityName { get; set; }
        public string HourlyJson { get; set; }
        public DateTime FetchedAt { get; set; }
    }
}

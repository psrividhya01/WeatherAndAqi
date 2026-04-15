namespace WeatherAPI.Models
{
    public class HourlyCache
    {
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string HourlyJson { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

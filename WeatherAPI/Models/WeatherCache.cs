namespace WeatherAPI.Models
{
    public class WeatherCache
    {
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string ResponseJson { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

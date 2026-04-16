using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class HourlyCache
    {
        [Key]
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string HourlyJson { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

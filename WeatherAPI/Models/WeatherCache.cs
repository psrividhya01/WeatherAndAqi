using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class WeatherCache
    {
        [Key]
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string ResponseJson { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

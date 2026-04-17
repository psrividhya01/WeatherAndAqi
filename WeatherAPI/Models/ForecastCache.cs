using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class ForecastCache
    {
        [Key]
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string ForecastJson { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System;

namespace WeatherAPI.Models
{
    public class AQICache
    {
        [Key]
        public int CacheId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string AQIJson { get; set; } = string.Empty;
        public int AQIScore { get; set; }
        public string AQICategory { get; set; } = string.Empty;
        public DateTime? FetchedAt { get; set; }
    }
}

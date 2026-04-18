using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class AQIHistory
    {
        [Key]
        public int HistoryId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int AQIScore { get; set; }
        public DateTime RecordedDate { get; set; }
    }
}

using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class AQITrendDto
    {
        public string City { get; set; } = string.Empty;
        public List<AQITrendEntryDto> TrendData { get; set; } = new();
    }

    public class AQITrendEntryDto
    {
        public DateTime RecordedDate { get; set; }
        public int AQIScore { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}

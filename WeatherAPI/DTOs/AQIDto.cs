using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class AQIDto
    {
        public string City { get; set; } = string.Empty;
        public int AQIScore { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Advisory { get; set; } = string.Empty;
        public string DominantPollutant { get; set; } = string.Empty;
        public Dictionary<string, PollutantDto> Pollutants { get; set; } = new();
    }
}

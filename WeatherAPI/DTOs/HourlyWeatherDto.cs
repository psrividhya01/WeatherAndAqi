using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class HourlyWeatherDto
    {
        public string City { get; set; } = string.Empty;
        public List<HourlyDataDto> Hours { get; set; } = new();
    }

    public class HourlyDataDto
    {
        public string Time { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

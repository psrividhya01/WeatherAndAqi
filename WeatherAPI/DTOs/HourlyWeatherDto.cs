using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class HourlyWeatherDto
    {
        public string City { get; set; } = string.Empty;
        public List<HourlyDataDto> Hours { get; set; } = new();
    }
}

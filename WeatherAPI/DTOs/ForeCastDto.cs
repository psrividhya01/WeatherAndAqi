using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class ForecastDto
    {
        public string City { get; set; } = string.Empty;
        public List<ForecastDayDto> Days { get; set; } = new();
    }
}

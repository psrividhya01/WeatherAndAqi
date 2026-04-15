using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class ForecastDto
    {
        public string City { get; set; } = string.Empty;
        public List<ForecastDayDto> Days { get; set; } = new();
    }

    public class ForecastDayDto
    {
        public string Date { get; set; } = string.Empty;
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

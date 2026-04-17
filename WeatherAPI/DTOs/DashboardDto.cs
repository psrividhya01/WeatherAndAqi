using System.Collections.Generic;

namespace WeatherAPI.DTOs
{
    public class DashboardDto
    {
        public string City { get; set; } = string.Empty;

        public CurrentWeatherDto? CurrentWeather { get; set; }

        public List<HourlyDataDto> Hourly { get; set; } = new();

        public List<ForecastDayDto> Forecast { get; set; } = new();

        public AQIDto? AQI { get; set; }
    }
}

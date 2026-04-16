using System.Collections.Generic;
using System;

namespace WeatherAPI.DTOs
{
    public class WeatherResponseDto
    {
        public string City { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Summary { get; set; } = string.Empty; // Enhanced: Human-readable insight
        public CurrentWeatherDto? CurrentWeather { get; set; }
        public List<HourlyDataDto> Hourly { get; set; } = new();
        public List<ForecastDayDto> Forecast { get; set; } = new();
        public AQIDto? AQI { get; set; }
    }
}

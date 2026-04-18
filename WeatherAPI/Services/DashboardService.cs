using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.DTOs;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Services
{
    public class DashboardService
    {
        private readonly IWeatherService _weatherService;
        private readonly IForecastService _forecastService; // Fixed: Use interface
        private readonly IAQIService _aqiService;         // Fixed: Use interface

        public DashboardService(
            IWeatherService weatherService,
            IForecastService forecastService,
            IAQIService aqiService)
        {
            _weatherService = weatherService;
            _forecastService = forecastService;
            _aqiService = aqiService;
        }

        public async Task<WeatherResponseDto> GetDashboardAsync(string city, double? lat, double? lon)
        {

            if (lat == null || lon == null)
            {
                var coordinates = await _weatherService.GetCoordinatesAsync(city!);
                if (coordinates == null)
                {
                    throw new System.Exception("Unable to retrieve location data for the specified city.");
                }
                lat = coordinates.Value.Latitude;
                lon = coordinates.Value.Longitude;
            }


            // 🚀 Call all services in parallel for performance
            var currentTask = _weatherService.GetCurrentWeatherAsync(city);
            var hourlyTask = _weatherService.GetHourlyWeatherAsync(city);
            var forecastTask = _forecastService.GetForecastAsync(city); // Fixed: Correct method name
            var aqiTask = _aqiService.GetAQIAsync(city);

            await Task.WhenAll(currentTask, hourlyTask, forecastTask, aqiTask);

            return new WeatherResponseDto
            {
                City = city,
                CurrentWeather = currentTask.Result,
                Hourly = hourlyTask.Result.Hours,
                Forecast = forecastTask.Result.Days,
                AQI = aqiTask.Result
            };
        }
    }
}

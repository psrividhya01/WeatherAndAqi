using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.DTOs;
using WeatherAPI.Interfaces;
using WeatherAPI.Mapping;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherCacheRepository _repo;
        private readonly IHourlyCacheRepository _hourlyRepo;
        private readonly IWeatherApiClient _api;
        private readonly IForecastService _forecastService;
        private readonly IAQIService _aqiService;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(
            IWeatherCacheRepository repo, 
            IHourlyCacheRepository hourlyRepo, 
            IWeatherApiClient api, 
            IForecastService forecastService,
            IAQIService aqiService,
            ILogger<WeatherService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hourlyRepo = hourlyRepo ?? throw new ArgumentNullException(nameof(hourlyRepo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
            _aqiService = aqiService ?? throw new ArgumentNullException(nameof(aqiService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CurrentWeatherDto> GetCurrentWeatherAsync(string cityName)
        {
            try
            {
                string jsonResponse;
                var cached = await _repo.GetCachedWeatherAsync(cityName);
                if (cached != null && !string.IsNullOrEmpty(cached.ResponseJson))
                {
                    _logger.LogInformation("Returning current weather for {CityName} from cache.", cityName);
                    jsonResponse = cached.ResponseJson;
                }
                else
                {
                    _logger.LogInformation("Cache miss for {CityName}. Fetching current weather from API.", cityName);
                    jsonResponse = await _api.GetWeatherAsync(cityName);
                    await _repo.SaveWeatherAsync(cityName, jsonResponse);
                }

                using var doc = JsonDocument.Parse(jsonResponse);
                return WeatherMapper.MapToCurrentWeatherDto(cityName, doc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching current weather for {CityName}", cityName);
                throw;
            }
        }

        public async Task<HourlyWeatherDto> GetHourlyWeatherAsync(string cityName)
        {
            try
            {
                string jsonResponse;
                var cached = await _hourlyRepo.GetCachedHourlyAsync(cityName);
                if (cached != null && !string.IsNullOrEmpty(cached.HourlyJson))
                {
                    _logger.LogInformation("Returning hourly weather for {CityName} from cache.", cityName);
                    jsonResponse = cached.HourlyJson;
                }
                else
                {
                    _logger.LogInformation("Cache miss for {CityName}. Fetching hourly weather from API.", cityName);
                    jsonResponse = await _api.GetHourlyWeatherAsync(cityName);
                    await _hourlyRepo.SaveHourlyAsync(cityName, jsonResponse);
                }

                using var doc = JsonDocument.Parse(jsonResponse);
                return WeatherMapper.MapToHourlyWeatherDto(cityName, doc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching hourly weather for {CityName}", cityName);
                throw;
            }
        }

        // UC: Unified Dashboard (Enhanced with smart logic)
        public async Task<WeatherResponseDto> GetWeatherDashboardAsync(string cityName)
        {
            _logger.LogInformation("Fetching unified dashboard data for {CityName}", cityName);

            // Sequential fetch to ensure thread safety for the Scoped DbContext
            var current = await GetCurrentWeatherAsync(cityName);
            var hourly = await GetHourlyWeatherAsync(cityName);
            var forecast = await _forecastService.GetForecastAsync(cityName);
            var aqi = await _aqiService.GetAQIAsync(cityName);

            // ENHANCEMENT: Generate a Smart Summary
            string summary = GenerateDashboardSummary(current, aqi);

            return new WeatherResponseDto
            {
                City = cityName,
                Timestamp = DateTime.Now,
                Summary = summary,
                CurrentWeather = current,
                Hourly = hourly.Hours,
                Forecast = forecast.Days,
                AQI = aqi
            };
        }

        private string GenerateDashboardSummary(CurrentWeatherDto weather, AQIDto aqi)
        {
            string weatherNote = weather.Temperature > 30 ? "It's a hot day." : 
                                 weather.Temperature < 10 ? "It's quite chilly." : "The temperature is pleasant.";
            
            string aqiNote = aqi.Status == "Good" ? "Air quality is excellent for outdoor activities." :
                             aqi.Status == "Moderate" ? "Air quality is acceptable." :
                             "Air quality is poor; consider limiting outdoor exposure.";

            return $"{weatherNote} {aqiNote} Don't forget to check the hourly timeline for changes!";
        }
    }
}

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
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IWeatherCacheRepository repo, IHourlyCacheRepository hourlyRepo, IWeatherApiClient api, ILogger<WeatherService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hourlyRepo = hourlyRepo ?? throw new ArgumentNullException(nameof(hourlyRepo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // UC1: GET /api/weather/current?city=
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

        // UC3: GET /api/weather/hourly?city=
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
    }
}

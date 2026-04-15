using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.ExternalServices;
using WeatherAPI.Repositories;
using WeatherAPI.Interfaces;
using WeatherAPI.DTOs;

namespace WeatherAPI.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherCacheRepository _repo;
        private readonly IWeatherApiClient _api;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(IWeatherCacheRepository repo, IWeatherApiClient api, ILogger<WeatherService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<CurrentWeatherDto> GetCurrentWeatherAsync(string cityName)
        {
            try
            {
                string jsonResponse;

                // 1. Check cache first
                var cached = await _repo.GetCachedWeatherAsync(cityName);
                if (cached != null && !string.IsNullOrEmpty(cached.ResponseJson))
                {
                    _logger.LogInformation("Returning weather data for {CityName} from cache.", cityName);
                    jsonResponse = cached.ResponseJson;
                }
                else
                {
                    _logger.LogInformation("Cache miss for {CityName}. Fetching from external API.", cityName);

                    // 2. If not in cache, call external API
                    jsonResponse = await _api.GetWeatherAsync(cityName);

                    // 3. Save to cache for future requests
                    await _repo.SaveWeatherAsync(cityName, jsonResponse);
                }

                // Parse the JSON
                var doc = JsonDocument.Parse(jsonResponse);

                var dto = new CurrentWeatherDto
                {
                    City = cityName,
                    Temperature = doc.RootElement.GetProperty("main").GetProperty("temp").GetDouble(),
                    FeelsLike = doc.RootElement.GetProperty("main").GetProperty("feels_like").GetDouble(),
                    Humidity = doc.RootElement.GetProperty("main").GetProperty("humidity").GetInt32(),
                    WindSpeed = doc.RootElement.GetProperty("wind").GetProperty("speed").GetDouble(),
                    ConditionCode = doc.RootElement.GetProperty("weather")[0].GetProperty("id").GetInt32(),
                    Description = doc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString() ?? string.Empty
                };

                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching weather data for {CityName}", cityName);
                throw; 
            }
        }
    }
}

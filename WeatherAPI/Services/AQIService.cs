using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.DTOs;
using WeatherAPI.Interfaces;
using WeatherAPI.Mapping;

namespace WeatherAPI.Services
{
    public class AQIService : IAQIService
    {
        private readonly IAQICacheRepository _repo;
        private readonly IAQIApiClient _api;
        private readonly IWeatherApiClient _weatherApi; // To get lat/lon from city name
        private readonly ILogger<AQIService> _logger;

        public AQIService(IAQICacheRepository repo, IAQIApiClient api, IWeatherApiClient weatherApi, ILogger<AQIService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _weatherApi = weatherApi ?? throw new ArgumentNullException(nameof(weatherApi));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<AQIDto> GetAQIAsync(string cityName)
        {
            try
            {
                string jsonResponse;
                var cached = await _repo.GetCachedAQIAsync(cityName);

                if (cached != null && !string.IsNullOrEmpty(cached.AQIJson))
                {
                    _logger.LogInformation("Returning AQI for {CityName} from cache.", cityName);
                    jsonResponse = cached.AQIJson;
                }
                else
                {
                    _logger.LogInformation("Cache miss for {CityName}. Fetching AQI from API.", cityName);
                    
                    // First get coordinates from weather API (simplest way since we already have it)
                    var weatherJson = await _weatherApi.GetWeatherAsync(cityName);
                    using var weatherDoc = JsonDocument.Parse(weatherJson);
                    double lat = weatherDoc.RootElement.GetProperty("coord").GetProperty("lat").GetDouble();
                    double lon = weatherDoc.RootElement.GetProperty("coord").GetProperty("lon").GetDouble();

                    jsonResponse = await _api.GetAQIAsync(lat, lon);
                    await _repo.SaveAQIAsync(cityName, jsonResponse);
                }

                using var doc = JsonDocument.Parse(jsonResponse);
                return AQIMapper.MapToAQIDto(cityName, doc);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching AQI for {CityName}", cityName);
                throw;
            }
        }
    }
}

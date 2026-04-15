using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.DTOs;

namespace WeatherAPI.Services
{
    public class ForecastService : IForecastService
    {
        private readonly IForecastCacheRepository _repo;
        private readonly IWeatherApiClient _api;
        private readonly ILogger<ForecastService> _logger;

        public ForecastService(IForecastCacheRepository repo, IWeatherApiClient api, ILogger<ForecastService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ForecastDto> GetForecastAsync(string cityName)
        {
            try
            {
                string jsonResponse;
                var cached = await _repo.GetCachedForecastAsync(cityName);
                
                if (cached != null && !string.IsNullOrEmpty(cached.ForecastJson))
                {
                    _logger.LogInformation("Returning forecast data for {CityName} from cache.", cityName);
                    jsonResponse = cached.ForecastJson;
                }
                else
                {
                    _logger.LogInformation("Cache miss for {CityName}. Fetching forecast from external API.", cityName);
                    jsonResponse = await _api.GetForecastAsync(cityName);
                    await _repo.SaveForecastAsync(cityName, jsonResponse);
                }

                // Placeholder mapping logic
                return new ForecastDto { City = cityName };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching forecast data for {CityName}", cityName);
                throw; 
            }
        }
    }
}

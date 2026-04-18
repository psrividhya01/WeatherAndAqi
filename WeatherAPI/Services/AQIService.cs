using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IWeatherApiClient _weatherApi;
        private readonly IAQIHistoryRepository _historyRepository;
        private readonly IAQIAdvisoryRepository _advisoryRepository;
        private readonly ILogger<AQIService> _logger;

        public AQIService(
            IAQICacheRepository repo,
            IAQIApiClient api,
            IWeatherApiClient weatherApi,
            IAQIHistoryRepository historyRepository,
            IAQIAdvisoryRepository advisoryRepository,
            ILogger<AQIService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _weatherApi = weatherApi ?? throw new ArgumentNullException(nameof(weatherApi));
            _historyRepository = historyRepository ?? throw new ArgumentNullException(nameof(historyRepository));
            _advisoryRepository = advisoryRepository ?? throw new ArgumentNullException(nameof(advisoryRepository));
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

        public async Task<List<AQIDto>> GetMultiAQIAsync(IEnumerable<string> cities)
        {
            if (cities == null)
            {
                return new List<AQIDto>();
            }

            var cityList = cities
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var tasks = cityList.Select(GetAQIAsync).ToList();
            await Task.WhenAll(tasks.Cast<Task>());
            return tasks.Select(t => t.Result).ToList();
        }

        public async Task<AQITrendDto> GetAQITrendAsync(string cityName, int days = 7)
        {
            var history = await _historyRepository.GetTrendAsync(cityName, days);
            var trendData = new List<AQITrendEntryDto>();

            foreach (var entry in history)
            {
                trendData.Add(new AQITrendEntryDto
                {
                    RecordedDate = entry.RecordedDate,
                    AQIScore = entry.AQIScore,
                    Category = GetAqiCategoryStatic(entry.AQIScore)
                });
            }

            return new AQITrendDto { City = cityName, TrendData = trendData };
        }

        public async Task<HealthAdvisoryDto> GetHealthAdvisoryAsync(int aqi)
        {
            var category = GetAqiCategoryStatic(aqi);
            var advisory = await _advisoryRepository.GetAdvisoryAsync(category);
            return AQIMapper.MapToHealthAdvisoryDto(advisory);
        }

        private static string GetAqiCategoryStatic(int aqi)
        {
            return aqi switch
            {
                <= 50 => "Good",
                <= 100 => "Moderate",
                <= 150 => "Sensitive",
                <= 200 => "Unhealthy",
                <= 300 => "Very Unhealthy",
                _ => "Hazardous"
            };
        }
    }
}

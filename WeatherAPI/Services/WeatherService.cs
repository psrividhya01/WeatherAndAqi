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
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherCacheRepository _repo;
        private readonly IHourlyCacheRepository _hourlyRepo;
        private readonly IWeatherApiClient _api;
        private readonly IForecastService _forecastService;
        private readonly IAQIService _aqiService;
        private readonly ICityCoordinateRepository _cityCoordinateRepository;
        private readonly ICachedCityWeatherRepository _cachedCityWeatherRepository;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(
            IWeatherCacheRepository repo, 
            IHourlyCacheRepository hourlyRepo, 
            IWeatherApiClient api, 
            IForecastService forecastService,
            IAQIService aqiService,
            ICityCoordinateRepository cityCoordinateRepository,
            ICachedCityWeatherRepository cachedCityWeatherRepository,
            ILogger<WeatherService> logger)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _hourlyRepo = hourlyRepo ?? throw new ArgumentNullException(nameof(hourlyRepo));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            _forecastService = forecastService ?? throw new ArgumentNullException(nameof(forecastService));
            _aqiService = aqiService ?? throw new ArgumentNullException(nameof(aqiService));
            _cityCoordinateRepository = cityCoordinateRepository ?? throw new ArgumentNullException(nameof(cityCoordinateRepository));
            _cachedCityWeatherRepository = cachedCityWeatherRepository ?? throw new ArgumentNullException(nameof(cachedCityWeatherRepository));
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

        public async Task<List<CityWeatherDto>> GetMultiCityWeatherAsync(IEnumerable<string> cities)
        {
            if (cities == null)
            {
                return new List<CityWeatherDto>();
            }

            var cityList = cities
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Take(4)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var weatherTasks = cityList.Select(GetCurrentWeatherAsync).ToList();
            var aqiTasks = cityList.Select(_aqiService.GetAQIAsync).ToList();

            await Task.WhenAll(weatherTasks.Cast<Task>().Concat(aqiTasks.Cast<Task>()));

            var result = new List<CityWeatherDto>();
            for (int i = 0; i < cityList.Count; i++)
            {
                var current = await weatherTasks[i];
                var aqi = await aqiTasks[i];

                result.Add(new CityWeatherDto
                {
                    City = current.City,
                    Temperature = current.Temperature,
                    Condition = current.Description,
                    Aqi = aqi.Aqi,
                    AqiCategory = aqi.Status,
                    Latitude = current.Latitude,
                    Longitude = current.Longitude,
                    Description = current.Description
                });
            }

            return result;
        }

        public async Task<List<CityWeatherDto>> GetNearbyCitiesAsync(double lat, double lon)
        {
            var nearbyCoordinates = await _cityCoordinateRepository.GetNearbyCoordinatesAsync(lat, lon, 8);
            if (nearbyCoordinates == null || !nearbyCoordinates.Any())
            {
                return new List<CityWeatherDto>();
            }

            return await GetMultiCityWeatherAsync(nearbyCoordinates.Select(c => c.CityName));
        }

        public async Task<List<SimilarCityDto>> GetSimilarCitiesAsync(int temp, string condition, string humidity)
        {
            var cachedCities = await _cachedCityWeatherRepository.GetAllCachedAsync();
            if (cachedCities == null || !cachedCities.Any())
            {
                await SeedCachedCityWeatherAsync();
                cachedCities = await _cachedCityWeatherRepository.GetAllCachedAsync();
            }

            var targetHumidity = ParseHumidity(humidity);

            var candidates = cachedCities
                .Where(c => string.IsNullOrWhiteSpace(condition) || c.Condition.Contains(condition, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (!candidates.Any())
            {
                candidates = cachedCities;
            }

            var ranked = candidates
                .Select(c => new SimilarCityDto
                {
                    City = c.CityName,
                    Temperature = c.Temperature,
                    Condition = c.Condition,
                    Humidity = c.Humidity,
                    SimilarityScore = Math.Abs(c.Temperature - temp) + Math.Abs(c.Humidity - targetHumidity) / 10.0,
                    MatchDescription = "Similar conditions"
                })
                .OrderBy(dto => dto.SimilarityScore)
                .Take(3)
                .ToList();

            return ranked;
        }

        private static int ParseHumidity(string humidity)
        {
            return humidity?.Trim().ToLowerInvariant() switch
            {
                "low" => 30,
                "medium" => 60,
                "high" => 85,
                _ => int.TryParse(humidity, out var parsed) ? parsed : 50
            };
        }

        private async Task SeedCachedCityWeatherAsync()
        {
            var seedCities = new[] { "New York", "London", "Paris", "Tokyo", "Sydney", "Dubai", "Singapore", "Mumbai", "Delhi", "Bangalore" };

            foreach (var city in seedCities)
            {
                try
                {
                    var current = await GetCurrentWeatherAsync(city);
                    var cached = new Models.CachedCityWeather
                    {
                        CityName = current.City,
                        Temperature = current.Temperature,
                        Condition = current.Description,
                        Humidity = current.Humidity,
                        FetchedAt = DateTime.UtcNow
                    };
                    await _cachedCityWeatherRepository.SaveCachedCityWeatherAsync(cached);
                }
                catch
                {
                    // Continue seeding even if one city fails.
                }
            }
        }

        private static string GetAqiCategory(int aqi)
        {
            return aqi switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown"
            };
        }

        // UC: Unified Dashboard (Aggregates everything for the frontend)
        public async Task<WeatherResponseDto> GetWeatherDashboardAsync(string cityName)
        {
            _logger.LogInformation("Fetching unified dashboard data for {CityName}", cityName);

            // Fetch sequential to avoid EF Core DbContext concurrency issues
            var current = await GetCurrentWeatherAsync(cityName);
            var hourly = await GetHourlyWeatherAsync(cityName);
            var forecast = await _forecastService.GetForecastAsync(cityName);
            var aqi = await _aqiService.GetAQIAsync(cityName);

            return new WeatherResponseDto
            {
                City = cityName,
                CurrentWeather = current,
                Hourly = hourly.Hours,
                Forecast = forecast.Days,
                AQI = aqi
            };
        }
    }
}

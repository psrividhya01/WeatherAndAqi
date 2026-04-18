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
                    
                    using var docTemp = JsonDocument.Parse(jsonResponse);
                    var dtoTemp = WeatherMapper.MapToCurrentWeatherDto(cityName, docTemp);
                    
                    await _repo.SaveWeatherAsync(cityName, jsonResponse, dtoTemp.Temperature, dtoTemp.Description, dtoTemp.Humidity, dtoTemp.Lat, dtoTemp.Lon);
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

        public async Task<CurrentWeatherDto> GetCurrentWeatherAsync(double lat, double lon)
        {
            string json = await _api.GetWeatherAsync(lat, lon);
            using var doc = JsonDocument.Parse(json);
            return WeatherMapper.MapToCurrentWeatherDto($"[{lat:F2}, {lon:F2}]", doc);
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

        public async Task<HourlyWeatherDto> GetHourlyWeatherAsync(double lat, double lon)
        {
            string json = await _api.GetHourlyWeatherAsync(lat, lon);
            using var doc = JsonDocument.Parse(json);
            return WeatherMapper.MapToHourlyWeatherDto($"[{lat:F2}, {lon:F2}]", doc);
        }


        // UC: Unified Dashboard (Enhanced with smart logic and parallelism)
        public async Task<WeatherResponseDto> GetWeatherDashboardAsync(string cityName)
        {
            var result = await GetDashboardAsync(null, null, cityName);
            
            return new WeatherResponseDto
            {
                City = result.City,
                Timestamp = DateTime.Now,
                Summary = GenerateDashboardSummary(result.CurrentWeather!, result.AQI!),
                CurrentWeather = result.CurrentWeather,
                Hourly = result.Hourly,
                Forecast = result.Forecast,
                AQI = result.AQI
            };
        }

        public async Task<DashboardDto> GetDashboardAsync(double? lat, double? lon, string? city)
        {
            if (lat == null || lon == null)
            {
                var coords = await _api.GetCityCoordinates(city!);
                lat = coords.lat;
                lon = coords.lon;
            }

            var currentTask = GetCurrentWeatherAsync(lat.Value, lon.Value);
            var forecastTask = _forecastService.GetForecastAsync(lat.Value, lon.Value);
            var hourlyTask = GetHourlyWeatherAsync(lat.Value, lon.Value);
            var aqiTask = _aqiService.GetAQIAsync(lat.Value, lon.Value);

            await Task.WhenAll(currentTask, forecastTask, hourlyTask, aqiTask);

            return new DashboardDto
            {
                City = city ?? $"[{lat.Value:F2}, {lon.Value:F2}]",
                CurrentWeather = currentTask.Result,
                Forecast = forecastTask.Result.Days,
                Hourly = hourlyTask.Result.Hours,
                AQI = aqiTask.Result
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

        public async Task<List<CurrentWeatherDto>> GetMultiCityWeatherAsync(string[] cities)
        {
            var tasks = cities.Select(city => GetCurrentWeatherAsync(city));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        public async Task<List<CurrentWeatherDto>> GetNearbyCitiesAsync(double lat, double lon)
        {
            // Dynamic nearby cities based on cached data within approx ~150km radius (simple coord delta)
            var caches = await _repo.GetRecentCachesAsync();
            
            return caches
                .Where(c => Math.Abs(c.Lat - lat) < 1.5 && Math.Abs(c.Lon - lon) < 1.5)
                .OrderBy(c => Math.Pow(c.Lat - lat, 2) + Math.Pow(c.Lon - lon, 2)) // Order by proximity
                .Take(5)
                .Select(c => {
                    using var doc = JsonDocument.Parse(c.ResponseJson);
                    return WeatherMapper.MapToCurrentWeatherDto(c.CityName, doc);
                })
                .ToList();
        }


        public List<CitySimilarityDto> FindSimilar(double temp, string condition, double humidity)
        {
            // Use repository to get recent caches and find similar ones
            var caches = _repo.GetRecentCachesAsync().Result; 
            
            return caches
                .Where(c => c.Condition.Contains(condition, StringComparison.OrdinalIgnoreCase))
                .Select(c => new
                {
                    Cache = c,
                    Score = Math.Abs(c.Temperature - temp) + Math.Abs(c.Humidity - humidity)
                })
                .OrderBy(x => x.Score)
                .Take(3)
                .Select(x => new CitySimilarityDto
                {
                    CityName = x.Cache.CityName,
                    Temperature = x.Cache.Temperature,
                    Condition = x.Cache.Condition,
                    SimilarityScore = x.Score
                })
                .ToList();
        }

        public async Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string city)
        {
            try
            {
                var coords = await _api.GetCityCoordinates(city);
                return (coords.lat, coords.lon);
            }
            catch
            {
                return null;
            }
        }

        public async Task<WeatherDigestDto> GetDailyDigestAsync(string city)
        {
            try
            {
                var weather = await GetCurrentWeatherAsync(city);
                var aqi = await _aqiService.GetAQIAsync(weather.Lat, weather.Lon);

                var summary = $"Today's Outlook for {city}: Temp {weather.Temperature}°C, " +
                              $"{weather.Description}, AQI {aqi.AQIScore} ({aqi.Category}). " +
                              "Stay hydrated and avoid peak sun hours.";

                return new WeatherDigestDto
                {
                    Summary = summary
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating daily digest for {City}", city);
                throw;
            }
        }

        private List<WeatherAlertDto> ExtractAlerts(JsonElement root)
        {
            return WeatherMapper.MapToAlerts(root);
        }
    }
}


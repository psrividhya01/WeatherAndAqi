using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;

namespace WeatherAPI.ExternalServices
{
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public WeatherApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["WeatherApi:ApiKey"] ?? throw new ArgumentNullException("WeatherApi:ApiKey is missing in appsettings.");
        }

        public async Task<string> GetWeatherAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetWeatherAsync(double lat, double lon)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetForecastAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetForecastAsync(double lat, double lon)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetHourlyWeatherAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetHourlyWeatherAsync(double lat, double lon)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetAQIData(string city)
        {
            var coords = await GetCityCoordinates(city);
            return await GetAQIData(coords.lat, coords.lon);
        }

        public async Task<string> GetAQIData(double lat, double lon)
        {
            var aqiUrl = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}";
            var response = await _httpClient.GetAsync(aqiUrl);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<(double lat, double lon)> GetCityCoordinates(string city)
        {
            var weatherUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}";
            var weatherResponse = await _httpClient.GetStringAsync(weatherUrl);
            
            using var doc = System.Text.Json.JsonDocument.Parse(weatherResponse);
            var lat = doc.RootElement.GetProperty("coord").GetProperty("lat").GetDouble();
            var lon = doc.RootElement.GetProperty("coord").GetProperty("lon").GetDouble();
            return (lat, lon);
        }
    }
}


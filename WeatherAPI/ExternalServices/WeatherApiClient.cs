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

        public async Task<string> GetForecastAsync(string cityName)
        {
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetHourlyWeatherAsync(string cityName)
        {
            // Uses free OWM 5-day/3-hour forecast endpoint; service takes first 8 items = next 24 hours
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&appid={_apiKey}&units=metric";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

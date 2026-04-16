using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;

namespace WeatherAPI.ExternalServices
{
    public class AQIApiClient : IAQIApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AQIApiClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = configuration["WeatherApi:ApiKey"] ?? throw new ArgumentNullException("WeatherApi:ApiKey is missing in appsettings.");
        }

        public async Task<string> GetAQIAsync(double lat, double lon)
        {
            var url = $"https://api.openweathermap.org/data/2.5/air_pollution?lat={lat}&lon={lon}&appid={_apiKey}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}

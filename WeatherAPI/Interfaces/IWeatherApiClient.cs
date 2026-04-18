using System.Threading.Tasks;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<string> GetWeatherAsync(string cityName);
        Task<string> GetWeatherAsync(double lat, double lon);
        Task<string> GetForecastAsync(string cityName);
        Task<string> GetForecastAsync(double lat, double lon);
        Task<string> GetHourlyWeatherAsync(string cityName);
        Task<string> GetHourlyWeatherAsync(double lat, double lon);
        Task<string> GetAQIData(string city);
        Task<string> GetAQIData(double lat, double lon);
        Task<(double lat, double lon)> GetCityCoordinates(string city);
    }
}


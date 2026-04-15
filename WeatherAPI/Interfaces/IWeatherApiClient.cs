using System.Threading.Tasks;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<string> GetWeatherAsync(string cityName);
        Task<string> GetForecastAsync(string cityName);
        Task<string> GetHourlyWeatherAsync(string cityName);
    }
}

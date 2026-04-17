using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherCacheRepository
    {
        Task<WeatherCache?> GetCachedWeatherAsync(string cityName);
        Task SaveWeatherAsync(string cityName, string json);
    }
}

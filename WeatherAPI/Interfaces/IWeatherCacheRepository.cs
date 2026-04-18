using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherCacheRepository
    {
        Task<WeatherCache?> GetCachedWeatherAsync(string cityName);
        Task SaveWeatherAsync(string cityName, string json, double temp, string condition, int humidity, double lat, double lon);
        Task<List<WeatherCache>> GetRecentCachesAsync();
    }
}



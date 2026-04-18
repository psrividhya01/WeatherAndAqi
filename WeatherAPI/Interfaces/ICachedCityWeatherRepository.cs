using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface ICachedCityWeatherRepository
    {
        Task<List<CachedCityWeather>> GetAllCachedAsync();
        Task SaveCachedCityWeatherAsync(CachedCityWeather cachedCityWeather);
    }
}

using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IForecastCacheRepository
    {
        Task<ForecastCache?> GetCachedForecastAsync(string cityName);
        Task SaveForecastAsync(string cityName, string json);
    }
}

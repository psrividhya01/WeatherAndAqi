using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IHourlyCacheRepository
    {
        Task<HourlyCache?> GetCachedHourlyAsync(string cityName);
        Task SaveHourlyAsync(string cityName, string json);
    }
}

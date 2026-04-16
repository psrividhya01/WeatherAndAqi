using System.Threading.Tasks;

namespace WeatherAPI.Interfaces
{
    public interface IAQICacheRepository
    {
        Task<Models.AQICache?> GetCachedAQIAsync(string cityName);
        Task SaveAQIAsync(string cityName, string json);
    }
}

using System.Threading.Tasks;

namespace WeatherAPI.Interfaces
{
    public interface IAQICacheRepository
    {
        // Renamed to match user's requested service logic
        Task<string?> GetCachedAQI(string city);
        Task SaveAQI(string city, string json);
    }
}

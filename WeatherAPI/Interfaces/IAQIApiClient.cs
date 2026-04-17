using System.Threading.Tasks;

namespace WeatherAPI.Interfaces
{
    public interface IAQIApiClient
    {
        Task<string> GetAQIAsync(double lat, double lon);
    }
}

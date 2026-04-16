using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IAQIService
    {
        Task<AQIDto> GetAQIAsync(string cityName);
    }
}

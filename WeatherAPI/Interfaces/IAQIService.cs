using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IAQIService
    {
        Task<AQIDto> GetAQIAsync(string cityName);
        Task<List<AQIDto>> GetMultiAQIAsync(IEnumerable<string> cities);
        Task<AQITrendDto> GetAQITrendAsync(string cityName, int days = 7);
        Task<HealthAdvisoryDto> GetHealthAdvisoryAsync(int aqi);
    }
}

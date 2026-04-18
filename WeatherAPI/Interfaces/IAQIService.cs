using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IAQIService
    {
        Task<AQIDto> GetAQIAsync(string cityName);
        Task<AQIDto> GetAQIAsync(double lat, double lon);
        Task<List<AQITrendDto>> GetAQITrendAsync(string city);
        Task<List<AQIDto>> GetMultiAQI(string[] cities);
    }
}



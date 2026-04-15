using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IForecastService
    {
        Task<ForecastDto> GetForecastAsync(string cityName);
    }
}

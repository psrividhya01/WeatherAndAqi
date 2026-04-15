using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherDto> GetCurrentWeatherAsync(string cityName);
        Task<HourlyWeatherDto> GetHourlyWeatherAsync(string cityName);
    }
}

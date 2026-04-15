using System.Threading.Tasks;
using WeatherAndAqi.DTOs;

namespace WeatherAndAqi.Interfaces;

public interface IWeatherService
{
    Task<WeatherResponseDto> GetWeatherDashboardAsync(string city);
}
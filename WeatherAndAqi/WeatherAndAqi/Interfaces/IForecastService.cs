using WeatherAndAqi.DTOs;

namespace WeatherAndAqi.Interfaces;

public interface IForecastService
{
    Task<ForecastDto> GetForecastAsync(string city);
}
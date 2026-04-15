using WeatherAndAqi.DTOs;

namespace WeatherAndAqi.Interfaces;

public interface IAQIService
{
    Task<AQIDto> GetAQIAsync(string city);
}
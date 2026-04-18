using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherDto> GetCurrentWeatherAsync(string cityName);
        Task<CurrentWeatherDto> GetCurrentWeatherAsync(double lat, double lon);
        Task<HourlyWeatherDto> GetHourlyWeatherAsync(string cityName);
        Task<HourlyWeatherDto> GetHourlyWeatherAsync(double lat, double lon);
        Task<WeatherResponseDto> GetWeatherDashboardAsync(string cityName);

        
        // Enhanced Methods
        Task<DashboardDto> GetDashboardAsync(double? lat, double? lon, string? city);
        Task<List<CurrentWeatherDto>> GetMultiCityWeatherAsync(string[] cities);
        Task<List<CurrentWeatherDto>> GetNearbyCitiesAsync(double lat, double lon);
        List<CitySimilarityDto> FindSimilar(double temp, string condition, double humidity);
        Task<(double Latitude, double Longitude)?> GetCoordinatesAsync(string city);
        Task<WeatherDigestDto> GetDailyDigestAsync(string city);
    }
}



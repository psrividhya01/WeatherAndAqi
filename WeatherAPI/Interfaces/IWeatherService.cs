using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.DTOs;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherService
    {
        Task<CurrentWeatherDto> GetCurrentWeatherAsync(string cityName);
        Task<HourlyWeatherDto> GetHourlyWeatherAsync(string cityName);
        Task<WeatherResponseDto> GetWeatherDashboardAsync(string cityName);
        Task<List<CityWeatherDto>> GetMultiCityWeatherAsync(IEnumerable<string> cities);
        Task<List<CityWeatherDto>> GetNearbyCitiesAsync(double lat, double lon);
        Task<List<SimilarCityDto>> GetSimilarCitiesAsync(int temp, string condition, string humidity);
    }
}

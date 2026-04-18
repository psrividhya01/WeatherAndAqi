using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface ICityCoordinateRepository
    {
        Task<List<CityCoordinate>> GetNearbyCoordinatesAsync(double lat, double lon, int maxResults = 8);
        Task<List<CityCoordinate>> GetAllCoordinatesAsync();
    }
}

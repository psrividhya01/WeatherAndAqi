using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IFavoriteCityRepository
    {
        Task<List<FavoriteCity>> GetFavoritesAsync(int userId);
        Task AddFavoriteAsync(FavoriteCity favorite);
        Task RemoveFavoriteAsync(int userId, string cityName);
        Task<bool> IsFavoriteAsync(int userId, string cityName);
    }
}

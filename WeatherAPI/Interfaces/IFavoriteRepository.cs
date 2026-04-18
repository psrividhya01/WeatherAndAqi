using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IFavoriteRepository
    {
        Task<IEnumerable<FavoriteCity>> GetFavoritesAsync(int userId);
        Task<FavoriteCity?> GetFavoriteAsync(int userId, string cityName);
        Task AddFavoriteAsync(FavoriteCity favorite);
        Task RemoveFavoriteAsync(FavoriteCity favorite);
        Task<bool> ExistsAsync(int userId, string cityName);
        Task<bool> SaveChangesAsync();
    }
}

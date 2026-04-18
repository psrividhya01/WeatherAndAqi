using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly AppDbContext _context;

        public FavoriteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoriteCity>> GetFavoritesAsync(int userId)
        {
            return await _context.FavoriteCities
                .Where(f => f.UserId == userId)
                .OrderBy(f => f.CityName)
                .ToListAsync();
        }

        public async Task<FavoriteCity?> GetFavoriteAsync(int userId, string cityName)
        {
            return await _context.FavoriteCities
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CityName == cityName);
        }

        public async Task AddFavoriteAsync(FavoriteCity favorite)
        {
            await _context.FavoriteCities.AddAsync(favorite);
        }

        public async Task RemoveFavoriteAsync(FavoriteCity favorite)
        {
            _context.FavoriteCities.Remove(favorite);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(int userId, string cityName)
        {
            return await _context.FavoriteCities
                .AnyAsync(f => f.UserId == userId && f.CityName == cityName);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}

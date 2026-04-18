using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class FavoriteCityRepository : IFavoriteCityRepository
    {
        private readonly AppDbContext _context;

        public FavoriteCityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FavoriteCity>> GetFavoritesAsync(int userId)
        {
            return await _context.FavoriteCities
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.SavedAt)
                .ToListAsync();
        }

        public async Task AddFavoriteAsync(FavoriteCity favorite)
        {
            var exists = await _context.FavoriteCities
                .AnyAsync(f => f.UserId == favorite.UserId && f.CityName == favorite.CityName);
            
            if (!exists)
            {
                favorite.SavedAt = DateTime.UtcNow;
                _context.FavoriteCities.Add(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveFavoriteAsync(int userId, string cityName)
        {
            var favorite = await _context.FavoriteCities
                .FirstOrDefaultAsync(f => f.UserId == userId && f.CityName == cityName);
            
            if (favorite != null)
            {
                _context.FavoriteCities.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsFavoriteAsync(int userId, string cityName)
        {
            return await _context.FavoriteCities
                .AnyAsync(f => f.UserId == userId && f.CityName == cityName);
        }
    }
}

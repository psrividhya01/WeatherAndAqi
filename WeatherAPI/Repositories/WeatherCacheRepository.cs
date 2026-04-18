using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class WeatherCacheRepository : IWeatherCacheRepository
    {
        private readonly AppDbContext _context;

        public WeatherCacheRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WeatherCache?> GetCachedWeatherAsync(string cityName)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-15);

            return await _context.WeatherCaches
                .Where(w => w.CityName == cityName && w.FetchedAt >= cutoff)
                .OrderByDescending(w => w.FetchedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveWeatherAsync(string cityName, string json, double temp, string condition, int humidity, double lat, double lon)
        {
            var cache = new WeatherCache
            {
                CityName = cityName,
                ResponseJson = json,
                Temperature = temp,
                Condition = condition,
                Humidity = humidity,
                Lat = lat,
                Lon = lon,
                FetchedAt = DateTime.UtcNow
            };

            _context.WeatherCaches.Add(cache);
            await _context.SaveChangesAsync();
        }


        public async Task<List<WeatherCache>> GetRecentCachesAsync()
        {
            var cutoff = DateTime.UtcNow.AddHours(-24);
            return await _context.WeatherCaches
                .Where(w => w.FetchedAt >= cutoff)
                .ToListAsync();
        }
    }
}


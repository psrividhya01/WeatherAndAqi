using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class AQICacheRepository : IAQICacheRepository
    {
        private readonly AppDbContext _context;

        public AQICacheRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AQICache?> GetCachedAQIAsync(string cityName)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-30); // AQI usually changes slower

            return await _context.AQICaches
                .Where(a => a.CityName == cityName && a.FetchedAt >= cutoff)
                .OrderByDescending(a => a.FetchedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAQIAsync(string cityName, string json)
        {
            var cache = new AQICache
            {
                CityName = cityName,
                AQIJson = json,
                FetchedAt = DateTime.UtcNow
            };

            _context.AQICaches.Add(cache);
            await _context.SaveChangesAsync();
        }
    }
}

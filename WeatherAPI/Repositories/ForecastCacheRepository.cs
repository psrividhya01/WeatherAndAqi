using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class ForecastCacheRepository : IForecastCacheRepository
    {
        private readonly AppDbContext _context;

        public ForecastCacheRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ForecastCache?> GetCachedForecastAsync(string cityName)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-15);

            return await _context.ForecastCaches
                .Where(f => f.CityName == cityName && f.FetchedAt >= cutoff)
                .OrderByDescending(f => f.FetchedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveForecastAsync(string cityName, string json)
        {
            var cache = new ForecastCache
            {
                CityName = cityName,
                ForecastJson = json,
                FetchedAt = DateTime.UtcNow
            };

            _context.ForecastCaches.Add(cache);
            await _context.SaveChangesAsync();
        }
    }
}

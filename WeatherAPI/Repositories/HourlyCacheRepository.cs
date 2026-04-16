using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class HourlyCacheRepository : IHourlyCacheRepository
    {
        private readonly AppDbContext _context;

        public HourlyCacheRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<HourlyCache?> GetCachedHourlyAsync(string cityName)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-15);

            return await _context.HourlyCaches
                .Where(h => h.CityName == cityName && h.FetchedAt >= cutoff)
                .OrderByDescending(h => h.FetchedAt)
                .FirstOrDefaultAsync();
        }

        public async Task SaveHourlyAsync(string cityName, string json)
        {
            var cache = new HourlyCache
            {
                CityName = cityName,
                HourlyJson = json,
                FetchedAt = DateTime.UtcNow
            };

            _context.HourlyCaches.Add(cache);
            await _context.SaveChangesAsync();
        }
    }
}

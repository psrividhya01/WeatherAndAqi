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

        public async Task<string?> GetCachedAQI(string city)
        {
            var cutoff = DateTime.UtcNow.AddMinutes(-30);

            var cache = await _context.AQICaches
                .Where(a => a.CityName == city && a.FetchedAt >= cutoff)
                .OrderByDescending(a => a.FetchedAt)
                .FirstOrDefaultAsync();

            return cache?.AQIJson;
        }

        public async Task SaveAQI(string city, string json)
        {
            // Parse for metadata storage
            using var doc = System.Text.Json.JsonDocument.Parse(json);
            var aqi = doc.RootElement.GetProperty("list")[0].GetProperty("main").GetProperty("aqi").GetInt32();

            var cache = new AQICache
            {
                CityName = city,
                AQIJson = json,
                AQIScore = aqi,
                FetchedAt = DateTime.UtcNow
            };

            _context.AQICaches.Add(cache);
            await _context.SaveChangesAsync();
        }
    }
}

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
    public class AQIHistoryRepository : IAQIHistoryRepository
    {
        private readonly AppDbContext _context;

        public AQIHistoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AQIHistory>> GetTrendAsync(string cityName, int days = 7)
        {
            var cutoff = DateTime.UtcNow.AddDays(-days);
            return await _context.AQIHistories
                .Where(h => h.CityName == cityName && h.RecordedDate >= cutoff)
                .OrderBy(h => h.RecordedDate)
                .ToListAsync();
        }

        public async Task SaveAQIHistoryAsync(AQIHistory history)
        {
            history.RecordedDate = DateTime.UtcNow;
            _context.AQIHistories.Add(history);
            await _context.SaveChangesAsync();
        }
    }
}

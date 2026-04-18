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
    public class WeatherAlertRepository : IWeatherAlertRepository
    {
        private readonly AppDbContext _context;

        public WeatherAlertRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherAlert>> GetActiveAlertsAsync(string cityName)
        {
            var cutoff = DateTime.UtcNow.AddDays(-1);
            return await _context.WeatherAlerts
                .Where(a => a.CityName == cityName && a.CreatedAt >= cutoff)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task SaveAlertAsync(WeatherAlert alert)
        {
            alert.CreatedAt = DateTime.UtcNow;
            _context.WeatherAlerts.Add(alert);
            await _context.SaveChangesAsync();
        }

        public async Task<List<WeatherAlert>> GetAlertHistoryAsync(string cityName, int days = 7)
        {
            var cutoff = DateTime.UtcNow.AddDays(-days);
            return await _context.WeatherAlerts
                .Where(a => a.CityName == cityName && a.CreatedAt >= cutoff)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}

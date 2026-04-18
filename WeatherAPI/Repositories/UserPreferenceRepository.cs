using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class UserPreferenceRepository : IUserPreferenceRepository
    {
        private readonly AppDbContext _context;

        public UserPreferenceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserPreference> GetPreferencesAsync(int userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId)
                ?? new UserPreference { UserId = userId };
        }

        public async Task SavePreferencesAsync(UserPreference preferences)
        {
            var existing = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == preferences.UserId);

            if (existing == null)
            {
                _context.UserPreferences.Add(preferences);
            }
            else
            {
                existing.PreferredUnit = preferences.PreferredUnit;
                existing.ShowForecast = preferences.ShowForecast;
                existing.ShowAQI = preferences.ShowAQI;
                existing.ShowMap = preferences.ShowMap;
                existing.ShowHourlyChart = preferences.ShowHourlyChart;
                existing.ShowHealthAdvisory = preferences.ShowHealthAdvisory;
                _context.UserPreferences.Update(existing);
            }

            await _context.SaveChangesAsync();
        }
    }
}

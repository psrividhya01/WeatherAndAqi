using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class CachedCityWeatherRepository : ICachedCityWeatherRepository
    {
        private readonly AppDbContext _context;

        public CachedCityWeatherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CachedCityWeather>> GetAllCachedAsync()
        {
            return await _context.CachedCityWeathers.ToListAsync();
        }

        public async Task SaveCachedCityWeatherAsync(CachedCityWeather cachedCityWeather)
        {
            _context.CachedCityWeathers.Add(cachedCityWeather);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class AQIAdvisoryRepository
    {
        private readonly AppDbContext _context;
        public AQIAdvisoryRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<AQIAdvisory?> GetAdvisory(string category)
        {
            return await _context.AQIAdvisories
                .FirstOrDefaultAsync(a => a.Category == category);
        }
    }
}

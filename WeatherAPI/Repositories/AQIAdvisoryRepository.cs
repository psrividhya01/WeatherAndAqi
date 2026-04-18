using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class AQIAdvisoryRepository : IAQIAdvisoryRepository
    {
        private readonly AppDbContext _context;

        public AQIAdvisoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AQIAdvisory> GetAdvisoryAsync(string category)
        {
            return await _context.AQIAdvisories
                .FirstOrDefaultAsync(a => a.Category == category) 
                ?? new AQIAdvisory 
                { 
                    Category = category,
                    Advisory = "No specific advisory available",
                    SensitiveGroupNote = "Please check official sources for updates"
                };
        }

        public async Task<List<AQIAdvisory>> GetAllAdvisoriesAsync()
        {
            return await _context.AQIAdvisories.ToListAsync();
        }
    }
}

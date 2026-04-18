using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPut("dashboard-config")]
        public async Task<IActionResult> SaveConfig([FromBody] string configJson)
        {
            var userId = 1; // demo
            var config = await _context.DashboardConfigs.FindAsync(userId);

            if (config != null)
            {
                config.ConfigJson = configJson;
            }
            else
            {
                _context.DashboardConfigs.Add(new DashboardConfig { UserId = userId, ConfigJson = configJson });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

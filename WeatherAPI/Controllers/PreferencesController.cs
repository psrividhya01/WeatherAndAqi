using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/preferences")]
    public class PreferencesController : ControllerBase
    {
        private readonly IUserPreferenceRepository _repository;

        public PreferencesController(IUserPreferenceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPreferences([FromQuery] int userId)
        {
            var prefs = await _repository.GetPreferencesAsync(userId);
            return Ok(prefs);
        }

        [HttpPut]
        public async Task<IActionResult> SavePreferences([FromBody] UserPreference preferences)
        {
            await _repository.SavePreferencesAsync(preferences);
            return Ok("Preferences saved");
        }
    }
}

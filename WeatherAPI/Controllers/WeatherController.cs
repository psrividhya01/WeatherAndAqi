using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")] // Simplified route to match user requirement
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        // GET /api/weather/current?city=CityName
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var data = await _service.GetCurrentWeatherAsync(city);
            return Ok(data);
        }

        // GET /api/weather/hourly?city=CityName
        [HttpGet("hourly")]
        public async Task<IActionResult> GetHourly([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var data = await _service.GetHourlyWeatherAsync(city);
            return Ok(data);
        }

        // GET /api/weather/dashboard?city=CityName
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            var data = await _service.GetWeatherDashboardAsync(city);
            return Ok(data);
        }
    }
}

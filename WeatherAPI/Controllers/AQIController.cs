using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/weather")]
    public class AQIController : ControllerBase
    {
        private readonly IAQIService _service;

        public AQIController(IAQIService service)
        {
            _service = service;
        }

        // GET /api/weather/aqi?city=Mumbai
        [HttpGet("aqi")]
        public async Task<IActionResult> GetAQI([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetAQIAsync(city);
            return Ok(data);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastController : ControllerBase
    {
        private readonly IForecastService _service;

        public ForecastController(IForecastService service)
        {
            _service = service;
        }

        [HttpGet("seven-day")]
        public async Task<IActionResult> GetForecast([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetForecastAsync(city);
            return Ok(data);
        }
    }
}

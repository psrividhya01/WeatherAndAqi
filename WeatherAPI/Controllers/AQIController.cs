using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.DTOs;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/aqi")]
    public class AQIController : ControllerBase
    {
        private readonly IAQIService _service;

        public AQIController(IAQIService service)
        {
            _service = service;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetAQI([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(ApiResponse<string>.Fail("City name is required."));

            var result = await _service.GetAQIAsync(city);
            return Ok(ApiResponse<AQIDto>.Ok(result));
        }

        [HttpGet("trend")]
        public async Task<IActionResult> GetTrend([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest(ApiResponse<string>.Fail("City name is required."));

            var result = await _service.GetAQITrendAsync(city);
            return Ok(ApiResponse<List<AQITrendDto>>.Ok(result));
        }
    }
}

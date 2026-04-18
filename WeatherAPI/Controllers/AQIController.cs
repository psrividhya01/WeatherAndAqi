using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public async Task<IActionResult> GetAQI(double? lat, double? lon, string? city)
        {
            if (!lat.HasValue && !lon.HasValue && string.IsNullOrWhiteSpace(city))
                return BadRequest(ApiResponse<string>.Fail("Either city or lat/lon must be provided."));

            AQIDto result;
            if (lat.HasValue && lon.HasValue)
                result = await _service.GetAQIAsync(lat.Value, lon.Value);
            else
                result = await _service.GetAQIAsync(city!);

            return Ok(ApiResponse<AQIDto>.Ok(result));
        }

        [HttpGet("multi")]
        public async Task<IActionResult> GetMultiAQI(string cities)
        {
            if (string.IsNullOrWhiteSpace(cities))
                return BadRequest(ApiResponse<string>.Fail("Cities list is required."));

            var result = await _service.GetMultiAQI(cities.Split(','));
            return Ok(ApiResponse<List<AQIDto>>.Ok(result));
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


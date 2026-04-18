using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        // GET /api/weather/aqi?city=Mumbai or /api/aqi?city=Mumbai
        [HttpGet("aqi")]
        [HttpGet("/api/aqi")]
        public async Task<IActionResult> GetAQI([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetAQIAsync(city);
            return Ok(data);
        }

        [HttpGet("aqi/multi")]
        [HttpGet("/api/aqi/multi")]
        public async Task<IActionResult> GetMultiAQI([FromQuery] string cities)
        {
            if (string.IsNullOrWhiteSpace(cities))
            {
                return BadRequest("At least one city is required.");
            }

            var cityList = cities.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (!cityList.Any())
            {
                return BadRequest("At least one valid city is required.");
            }

            var data = await _service.GetMultiAQIAsync(cityList);
            return Ok(data);
        }

        [HttpGet("aqi/trend")]
        public async Task<IActionResult> GetAQITrend([FromQuery] string city, [FromQuery] int days = 7)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var trend = await _service.GetAQITrendAsync(city, days);
            return Ok(trend);
        }

        [HttpGet("aqi/advisory")]
        public async Task<IActionResult> GetHealthAdvisory([FromQuery] int aqi)
        {
            var advisory = await _service.GetHealthAdvisoryAsync(aqi);
            return Ok(advisory);
        }
    }
}

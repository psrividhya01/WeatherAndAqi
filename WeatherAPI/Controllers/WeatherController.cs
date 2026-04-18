using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrent([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetCurrentWeatherAsync(city);
            return Ok(data);
        }

        [HttpGet("hourly")]
        public async Task<IActionResult> GetHourly([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetHourlyWeatherAsync(city);
            return Ok(data);
        }

        [HttpGet("multi")]
        public async Task<IActionResult> GetMulti([FromQuery] string cities)
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

            var data = await _service.GetMultiCityWeatherAsync(cityList);
            return Ok(data);
        }

        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby([FromQuery] double lat, [FromQuery] double lon)
        {
            var data = await _service.GetNearbyCitiesAsync(lat, lon);
            return Ok(data);
        }

        [HttpGet("similar")]
        public async Task<IActionResult> GetSimilar([FromQuery] int temp, [FromQuery] string condition, [FromQuery] string humidity)
        {
            if (string.IsNullOrWhiteSpace(condition))
            {
                return BadRequest("Condition is required.");
            }

            var data = await _service.GetSimilarCitiesAsync(temp, condition, humidity);
            return Ok(data);
        }

        // GET /api/weather/dashboard?city=Mumbai (Connects all data to frontend)
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name is required.");
            }

            var data = await _service.GetWeatherDashboardAsync(city);
            return Ok(data);
        }
    }
}

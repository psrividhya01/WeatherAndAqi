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

        // GET /api/weather/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard(double? lat, double? lon, string? city)
        {
            if (!lat.HasValue && !lon.HasValue && string.IsNullOrWhiteSpace(city))
                return BadRequest("Either city or lat/lon must be provided.");

            var result = await _service.GetDashboardAsync(lat, lon, city);
            return Ok(result);
        }

        // GET /api/weather/multi?cities=London,Paris,Berlin
        [HttpGet("multi")]
        public async Task<IActionResult> GetMultiCityWeather(string cities)
        {
            if (string.IsNullOrWhiteSpace(cities))
                return BadRequest("Cities list is required.");

            var cityList = cities.Split(',');
            var result = await _service.GetMultiCityWeatherAsync(cityList);
            return Ok(result);
        }

        // GET /api/weather/nearby?lat=...&lon=...
        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearby(double lat, double lon)
        {
            var result = await _service.GetNearbyCitiesAsync(lat, lon);
            return Ok(result);
        }

        // GET /api/weather/similar?temp=...&condition=...&humidity=...
        [HttpGet("similar")]
        public IActionResult GetSimilar(double temp, string condition, double humidity)
        {
            var result = _service.FindSimilar(temp, condition, humidity);
            return Ok(result);
        }

        [HttpGet("digest")]
        public async Task<IActionResult> GetWeatherDigest(string city)
        {
            var digest = await _service.GetDailyDigestAsync(city);
            return Ok(digest);
        }

    }
}

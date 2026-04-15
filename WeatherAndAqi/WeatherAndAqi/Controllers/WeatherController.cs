
using Microsoft.AspNetCore.Mvc;
using WeatherAndAqi.Interfaces;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboard(string city)
    {
        var result = await _weatherService.GetWeatherDashboardAsync(city);
        return Ok(result);
    }
}
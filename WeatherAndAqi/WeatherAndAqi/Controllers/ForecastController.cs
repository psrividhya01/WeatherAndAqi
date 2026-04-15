using Microsoft.AspNetCore.Mvc;
using WeatherAndAqi.Interfaces;

namespace WeatherAndAqi.Controllers;

[ApiController]
[Route("api/forecast")]
public class ForecastController : ControllerBase
{
    private readonly IForecastService _forecastService;

    public ForecastController(IForecastService forecastService)
    {
        _forecastService = forecastService;
    }

    [HttpGet]
    public async Task<IActionResult> GetForecast(string city)
    {
        var result = await _forecastService.GetForecastAsync(city);
        return Ok(result);
    }
}
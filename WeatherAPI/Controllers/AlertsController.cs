using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/alerts")]
    public class AlertsController : ControllerBase
    {
        private readonly IWeatherAlertRepository _repository;

        public AlertsController(IWeatherAlertRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveAlerts([FromQuery] string city)
        {
            var alerts = await _repository.GetActiveAlertsAsync(city);
            return Ok(alerts);
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetAlertHistory([FromQuery] string city, [FromQuery] int days = 7)
        {
            var history = await _repository.GetAlertHistoryAsync(city, days);
            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAlert([FromBody] WeatherAlert alert)
        {
            await _repository.SaveAlertAsync(alert);
            return Ok("Alert saved");
        }
    }
}

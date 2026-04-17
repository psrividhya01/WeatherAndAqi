using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;
using WeatherAPI.Data;

namespace WeatherAPI.BackgroundServices
{
    public class AQIBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<AQIBackgroundService> _logger;

        public AQIBackgroundService(IServiceProvider services, ILogger<AQIBackgroundService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("AQI History Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await RecordHistoryAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while recording AQI history.");
                }

                // Wait 6 hours between snapshots
                await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
            }
        }

        private async Task RecordHistoryAsync()
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var weatherApi = scope.ServiceProvider.GetRequiredService<IWeatherApiClient>();

            // For simplicity, we track history for a core set of cities
            var cities = new List<string> { "Mumbai", "London", "New York", "Tokyo", "Chennai" };

            foreach (var city in cities)
            {
                try
                {
                    _logger.LogInformation("Recording history snapshot for {City}", city);
                    
                    var data = await weatherApi.GetAQIData(city);
                    using var doc = System.Text.Json.JsonDocument.Parse(data);
                    var main = doc.RootElement.GetProperty("list")[0].GetProperty("main");
                    int aqi = main.GetProperty("aqi").GetInt32();

                    var history = new AQIHistory
                    {
                        CityName = city,
                        AQIScore = aqi, // Storing raw 1-5 index for trend
                        RecordedDate = DateTime.UtcNow
                    };

                    context.AQIHistories.Add(history);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Failed to record history for {City}: {Msg}", city, ex.Message);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}

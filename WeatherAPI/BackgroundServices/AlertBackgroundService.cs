using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.BackgroundServices
{
    public class AlertBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AlertBackgroundService> _logger;

        public AlertBackgroundService(IServiceProvider serviceProvider, ILogger<AlertBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        var weatherApi = scope.ServiceProvider.GetRequiredService<IWeatherApiClient>();
                        
                        // Check for alerts in major cities (demo cities)
                        string[] cities = { "Mumbai", "Delhi", "London", "New York" };
                        foreach (var city in cities)
                        {
                            var json = await weatherApi.GetWeatherAsync(city);
                            using var doc = JsonDocument.Parse(json);
                            var root = doc.RootElement;
                            
                            if (root.TryGetProperty("alerts", out var alertsElement))
                            {
                                foreach (var alert in alertsElement.EnumerateArray())
                                {
                                    var alertType = alert.GetProperty("event").GetString() ?? "Unknown";
                                    var message = alert.GetProperty("description").GetString() ?? "";
                                    
                                    if (!context.WeatherAlerts.Any(a => a.CityName == city && a.Message == message))
                                    {
                                        context.WeatherAlerts.Add(new WeatherAlert
                                        {
                                            CityName = city,
                                            AlertType = alertType,
                                            Severity = "High",
                                            Message = message,
                                            CreatedAt = DateTime.UtcNow
                                        });
                                    }
                                }
                            }
                        }
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in AlertBackgroundService");
                }
                await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
            }
        }
    }
}

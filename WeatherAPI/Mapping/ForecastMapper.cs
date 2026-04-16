using System.Collections.Generic;
using System.Text.Json;
using WeatherAPI.DTOs;

namespace WeatherAPI.Mapping
{
    public static class ForecastMapper
    {
        // UC2: Maps raw OWM 3-hourly forecast JSON → ForecastDto
        // Takes every 8th item (8 × 3hrs = 24hrs) → 1 entry per day
        public static ForecastDto MapToForecastDto(string cityName, JsonDocument doc)
        {
            var list = doc.RootElement.GetProperty("list");
            var days = new List<ForecastDayDto>();

            for (int i = 0; i < list.GetArrayLength(); i += 8)
            {
                var item = list[i];
                var main = item.GetProperty("main");
                var weather = item.GetProperty("weather")[0];

                days.Add(new ForecastDayDto
                {
                    Date = item.GetProperty("dt_txt").GetString() ?? string.Empty,
                    MinTemp = main.GetProperty("temp_min").GetDouble(),
                    MaxTemp = main.GetProperty("temp_max").GetDouble(),
                    Description = weather.GetProperty("description").GetString() ?? string.Empty,
                    ConditionCode = weather.GetProperty("id").GetInt32(),
                    PrecipitationProbability = item.TryGetProperty("pop", out var pop) ? pop.GetDouble() * 100 : 0
                });
            }

            return new ForecastDto { City = cityName, Days = days };
        }
    }
}

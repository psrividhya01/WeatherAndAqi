using System;
using System.Collections.Generic;
using System.Text.Json;
using WeatherAPI.DTOs;

namespace WeatherAPI.Mapping
{
    public static class AQIMapper
    {
        public static AQIDto MapToAQIDto(string cityName, JsonDocument doc)
        {
            var list = doc.RootElement.GetProperty("list")[0];
            var main = list.GetProperty("main");
            var components = list.GetProperty("components");

            int aqiValue = main.GetProperty("aqi").GetInt32();

            var pollutants = new Dictionary<string, PollutantDto>
            {
                ["pm25"] = new PollutantDto { CurrentValue = components.GetProperty("pm2_5").GetDouble(), WHOlimit = 25 },
                ["pm10"] = new PollutantDto { CurrentValue = components.GetProperty("pm10").GetDouble(), WHOlimit = 50 },
                ["co"] = new PollutantDto { CurrentValue = components.GetProperty("co").GetDouble(), WHOlimit = 4 },
                ["no2"] = new PollutantDto { CurrentValue = components.GetProperty("no2").GetDouble(), WHOlimit = 40 },
                ["so2"] = new PollutantDto { CurrentValue = components.GetProperty("so2").GetDouble(), WHOlimit = 20 },
                ["o3"] = new PollutantDto { CurrentValue = components.GetProperty("o3").GetDouble(), WHOlimit = 100 }
            };

            return new AQIDto
            {
                City = cityName,
                AQIScore = aqiValue, // Matches the new property name
                Status = GetAqiStatus(aqiValue),
                Category = GetAqiStatus(aqiValue),
                Pollutants = pollutants
            };
        }

        private static string GetAqiStatus(int aqi)
        {
            return aqi switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown"
            };
        }
    }
}

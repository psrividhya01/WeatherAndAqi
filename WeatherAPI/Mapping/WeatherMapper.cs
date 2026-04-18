using System;
using System.Collections.Generic;
using System.Text.Json;
using WeatherAPI.DTOs;

namespace WeatherAPI.Mapping
{
    public static class WeatherMapper
    {
        // UC1: Maps raw OWM current weather JSON → CurrentWeatherDto
        public static CurrentWeatherDto MapToCurrentWeatherDto(string cityName, JsonDocument doc)
        {
            var root = doc.RootElement;

            return new CurrentWeatherDto
            {
                City = cityName,
                Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                FeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                Visibility = root.TryGetProperty("visibility", out var vis) ? vis.GetInt32() : 0,
                ConditionCode = root.GetProperty("weather")[0].GetProperty("id").GetInt32(),
                Description = root.GetProperty("weather")[0].GetProperty("description").GetString() ?? string.Empty,
                Latitude = root.GetProperty("coord").GetProperty("lat").GetDouble(),
                Longitude = root.GetProperty("coord").GetProperty("lon").GetDouble()
            };
        }

        // UC3: Maps raw OWM 3-hourly JSON → HourlyWeatherDto (first 8 items = next 24 hours)
        public static HourlyWeatherDto MapToHourlyWeatherDto(string cityName, JsonDocument doc)
        {
            var list = doc.RootElement.GetProperty("list");
            var hours = new List<HourlyDataDto>();

            for (int i = 0; i < 8 && i < list.GetArrayLength(); i++)
            {
                var item = list[i];
                hours.Add(new HourlyDataDto
                {
                    Time = DateTimeOffset.FromUnixTimeSeconds(item.GetProperty("dt").GetInt64()).DateTime,
                    Temperature = item.GetProperty("main").GetProperty("temp").GetDouble(),
                    PrecipitationProbability = item.TryGetProperty("pop", out var pop) ? pop.GetDouble() * 100 : 0,
                    ConditionCode = item.GetProperty("weather")[0].GetProperty("id").GetInt32()
                });
            }

            return new HourlyWeatherDto { City = cityName, Hours = hours };
        }
    }
}

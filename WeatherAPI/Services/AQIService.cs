using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.DTOs;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Services
{
    public class AQIService : IAQIService
    {
        private readonly IAQICacheRepository _repo;
        private readonly IWeatherApiClient _api;
        private readonly AppDbContext _context;

        public AQIService(IAQICacheRepository repo, IWeatherApiClient api, AppDbContext context)
        {
            _repo = repo;
            _api = api;
            _context = context;
        }

        public async Task<AQIDto> GetAQIAsync(string city)
        {
            var cached = await _repo.GetCachedAQI(city);
            string json;

            if (cached != null)
            {
                json = cached;
            }
            else
            {
                json = await _api.GetAQIData(city);
                await _repo.SaveAQI(city, json);
            }

            return await ParseAQIJson(city, json);
        }

        public async Task<AQIDto> GetAQIAsync(double lat, double lon)
        {
            string json = await _api.GetAQIData(lat, lon);
            // Optional: We could try to resolve the city name from the JSON if available
            using var doc = JsonDocument.Parse(json);
            string cityName = $"[{lat:F2}, {lon:F2}]";
            return await ParseAQIJson(cityName, json);
        }

        public async Task<List<AQIDto>> GetMultiAQI(string[] cities)
        {
            var tasks = cities.Select(city => GetAQIAsync(city));
            var results = await Task.WhenAll(tasks);
            return results.ToList();
        }

        private async Task<AQIDto> ParseAQIJson(string city, string json)
        {
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement.GetProperty("list")[0];
            var aqiRaw = root.GetProperty("main").GetProperty("aqi").GetInt32();
            var components = root.GetProperty("components");

            var category = GetProperAQICategory(aqiRaw);
            var pollutantMaster = await _context.PollutantInfos.ToListAsync();
            var advisory = await _context.AQIAdvisories
                .FirstOrDefaultAsync(a => a.Category == category);

            var pollutants = new Dictionary<string, PollutantDto>
            {
                ["pm25"] = MapPollutant("pm2_5", components, 25, pollutantMaster),
                ["pm10"] = MapPollutant("pm10", components, 50, pollutantMaster),
                ["co"] = MapPollutant("co", components, 4, pollutantMaster),
                ["no2"] = MapPollutant("no2", components, 40, pollutantMaster),
                ["so2"] = MapPollutant("so2", components, 20, pollutantMaster),
                ["o3"] = MapPollutant("o3", components, 100, pollutantMaster)
            };

            var dominant = pollutants
                .OrderByDescending(p => p.Value.CurrentValue / (p.Value.WHOlimit == 0 ? 1 : p.Value.WHOlimit))
                .First().Key;

            return new AQIDto
            {
                City = city,
                AQIScore = MapIndexToScore(aqiRaw),
                Category = category,
                Status = category,
                DominantPollutant = dominant,
                Pollutants = pollutants,
                Advisory = advisory?.Advisory ?? "No advisory available."
            };
        }


        public async Task<List<AQITrendDto>> GetAQITrendAsync(string city)
        {
            var history = await _context.AQIHistories
                .Where(h => h.CityName == city)
                .OrderByDescending(h => h.RecordedDate)
                .Take(28) // Last 7 days (4 snapshots per day)
                .Select(h => new AQITrendDto
                {
                    date = h.RecordedDate,
                    AQIScore = MapIndexToScore(h.AQIScore)
                })
                .ToListAsync();

            return history.OrderBy(h => h.date).ToList();
        }

        private PollutantDto MapPollutant(string key, JsonElement components, double limit, List<Models.PollutantInfo> master)
        {
            var info = master.FirstOrDefault(m => m.PollutantCode == key.Replace("_", ""));
            return new PollutantDto
            {
                CurrentValue = components.GetProperty(key).GetDouble(),
                WHOlimit = limit
                // Description = info?.Description (Add this to DTO if needed)
            };
        }

        private string GetProperAQICategory(int aqiIndex)
        {
            return aqiIndex switch
            {
                1 => "Good",
                2 => "Fair",
                3 => "Moderate",
                4 => "Poor",
                5 => "Very Poor",
                _ => "Unknown"
            };
        }

        private int MapIndexToScore(int index)
        {
            return index switch { 1 => 45, 2 => 90, 3 => 140, 4 => 190, 5 => 280, _ => 0 };
        }
    }
}

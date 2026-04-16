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

            return new AQIDto
            {
                City = cityName,
                Aqi = aqiValue,
                Status = GetAqiStatus(aqiValue),
                Co = components.GetProperty("co").GetDouble(),
                No2 = components.GetProperty("no2").GetDouble(),
                O3 = components.GetProperty("o3").GetDouble(),
                So2 = components.GetProperty("so2").GetDouble(),
                Pm2_5 = components.GetProperty("pm2_5").GetDouble(),
                Pm10 = components.GetProperty("pm10").GetDouble()
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

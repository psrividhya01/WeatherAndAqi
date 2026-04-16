using Microsoft.EntityFrameworkCore;
using WeatherAPI.Models;

namespace WeatherAPI.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            // 1. Seed Pollutant Info
            if (!context.PollutantInfos.Any())
            {
                context.PollutantInfos.AddRange(
                    new PollutantInfo { PollutantCode = "pm25", FullName = "PM2.5", Description = "Fine particulate matter that can penetrate deep into lungs.", CommonSources = "Vehicles, Burning fossil fuels, forest fires." },
                    new PollutantInfo { PollutantCode = "pm10", FullName = "PM10", Description = "Inhalable coarse particles.", CommonSources = "Dust from roads, construction, industrial emissions." },
                    new PollutantInfo { PollutantCode = "co", FullName = "Carbon Monoxide", Description = "Colorless, odorless gas.", CommonSources = "Incomplete combustion, vehicle exhaust." },
                    new PollutantInfo { PollutantCode = "no2", FullName = "Nitrogen Dioxide", Description = "Highly reactive gas formed from fuel burning.", CommonSources = "Power plants, vehicle engines." },
                    new PollutantInfo { PollutantCode = "so2", FullName = "Sulfur Dioxide", Description = "Sharp-smelling gas formed from coal/oil burning.", CommonSources = "Power plants, industrial boilers." },
                    new PollutantInfo { PollutantCode = "o3", FullName = "Ozone", Description = "Ground-level ozone formed by chemical reactions.", CommonSources = "Sunlight reacting with NO2 and VOCs." }
                );
            }

            // 2. Seed AQI Advisories
            if (!context.AQIAdvisories.Any())
            {
                context.AQIAdvisories.AddRange(
                    new AQIAdvisory { Category = "Good", Advisory = "Air quality is considered satisfactory, and air pollution poses little or no risk.", SensitiveGroupNote = "Enjoy your outdoor activities!" },
                    new AQIAdvisory { Category = "Fair", Advisory = "Air quality is acceptable.", SensitiveGroupNote = "Sensitive individuals should consider reducing heavy exertion outdoors." },
                    new AQIAdvisory { Category = "Moderate", Advisory = "Members of sensitive groups may experience health effects.", SensitiveGroupNote = "Children and active adults should limit prolonged outdoor exertion." },
                    new AQIAdvisory { Category = "Poor", Advisory = "Everyone may begin to experience health effects.", SensitiveGroupNote = "Avoid prolonged outdoor exertion; wear a mask if necessary." },
                    new AQIAdvisory { Category = "Very Poor", Advisory = "Health warnings of emergency conditions. The entire population is more likely to be affected.", SensitiveGroupNote = "Stay indoors and keep windows closed." }
                );
            }

            context.SaveChanges();
        }
    }
}

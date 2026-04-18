using Microsoft.EntityFrameworkCore;
using WeatherAPI.Models;

namespace WeatherAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<WeatherCache> WeatherCaches { get; set; }
        public DbSet<ForecastCache> ForecastCaches { get; set; }
        public DbSet<HourlyCache> HourlyCaches { get; set; }
        public DbSet<AQICache> AQICaches { get; set; }
        public DbSet<AQIAdvisory> AQIAdvisories { get; set; }
        public DbSet<AQIHistory> AQIHistories { get; set; }
        public DbSet<PollutantInfo> PollutantInfos { get; set; }
        public DbSet<FavoriteCity> FavoriteCities { get; set; }
        public DbSet<DashboardConfig> DashboardConfigs { get; set; }
        public DbSet<WeatherAlert> WeatherAlerts { get; set; }
    }
}

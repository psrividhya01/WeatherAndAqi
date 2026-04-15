using Dapper;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Models;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Repositories
{
    public class ForecastCacheRepository : IForecastCacheRepository
    {
        private readonly SqlDbConnectionFactory _dbConnectionFactory;

        public ForecastCacheRepository(SqlDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<ForecastCache?> GetCachedForecastAsync(string cityName)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"SELECT TOP 1 ForecastJson 
                          FROM ForecastCache 
                          WHERE CityName = @CityName 
                          AND DATEDIFF(MINUTE, FetchedAt, GETDATE()) < 15
                          ORDER BY FetchedAt DESC";
            
            return await connection.QueryFirstOrDefaultAsync<ForecastCache>(query, new { CityName = cityName });
        }

        public async Task SaveForecastAsync(string cityName, string json)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"INSERT INTO ForecastCache (CityName, ForecastJson, FetchedAt)
                          VALUES (@CityName, @ForecastJson, GETDATE())";
                          
            await connection.ExecuteAsync(query, new { CityName = cityName, ForecastJson = json });
        }
    }
}

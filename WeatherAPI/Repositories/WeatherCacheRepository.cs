using Dapper;
using System.Data;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Models;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Repositories
{
    public class WeatherCacheRepository : IWeatherCacheRepository
    {
        private readonly SqlDbConnectionFactory _dbConnectionFactory;

        public WeatherCacheRepository(SqlDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<WeatherCache?> GetCachedWeatherAsync(string cityName)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"SELECT TOP 1 ResponseJson 
                      FROM WeatherCache 
                      WHERE CityName = @CityName 
                      AND DATEDIFF(MINUTE, FetchedAt, GETDATE()) < 15
                      ORDER BY FetchedAt DESC";
            
            return await connection.QueryFirstOrDefaultAsync<WeatherCache>(query, new { CityName = cityName });
        }

        public async Task SaveWeatherAsync(string cityName, string json)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"INSERT INTO WeatherCache (CityName, ResponseJson, FetchedAt)
                      VALUES (@CityName, @ResponseJson, GETDATE())";
                      
            await connection.ExecuteAsync(query, new { CityName = cityName, ResponseJson = json });
        }
    }
}

using Dapper;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Models;
using WeatherAPI.Interfaces;

namespace WeatherAPI.Repositories
{
    public class HourlyCacheRepository : IHourlyCacheRepository
    {
        private readonly SqlDbConnectionFactory _dbConnectionFactory;

        public HourlyCacheRepository(SqlDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<HourlyCache?> GetCachedHourlyAsync(string cityName)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"SELECT TOP 1 HourlyJson 
                          FROM HourlyCache 
                          WHERE CityName = @CityName 
                          AND DATEDIFF(MINUTE, FetchedAt, GETDATE()) < 15
                          ORDER BY FetchedAt DESC";
            
            return await connection.QueryFirstOrDefaultAsync<HourlyCache>(query, new { CityName = cityName });
        }

        public async Task SaveHourlyAsync(string cityName, string json)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            var query = @"INSERT INTO HourlyCache (CityName, HourlyJson, FetchedAt)
                          VALUES (@CityName, @HourlyJson, GETDATE())";
                          
            await connection.ExecuteAsync(query, new { CityName = cityName, HourlyJson = json });
        }
    }
}

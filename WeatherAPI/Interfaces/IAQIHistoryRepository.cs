using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IAQIHistoryRepository
    {
        Task<List<AQIHistory>> GetTrendAsync(string cityName, int days = 7);
        Task SaveAQIHistoryAsync(AQIHistory history);
    }
}

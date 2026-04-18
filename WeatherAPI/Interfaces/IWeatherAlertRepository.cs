using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IWeatherAlertRepository
    {
        Task<List<WeatherAlert>> GetActiveAlertsAsync(string cityName);
        Task SaveAlertAsync(WeatherAlert alert);
        Task<List<WeatherAlert>> GetAlertHistoryAsync(string cityName, int days = 7);
    }
}

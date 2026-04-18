using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IAQIAdvisoryRepository
    {
        Task<AQIAdvisory> GetAdvisoryAsync(string category);
        Task<List<AQIAdvisory>> GetAllAdvisoriesAsync();
    }
}

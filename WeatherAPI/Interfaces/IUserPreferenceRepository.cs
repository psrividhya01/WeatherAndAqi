using System.Threading.Tasks;
using WeatherAPI.Models;

namespace WeatherAPI.Interfaces
{
    public interface IUserPreferenceRepository
    {
        Task<UserPreference> GetPreferencesAsync(int userId);
        Task SavePreferencesAsync(UserPreference preferences);
    }
}

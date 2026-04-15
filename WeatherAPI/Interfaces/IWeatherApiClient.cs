namespace WeatherAPI.Interfaces
{
    public interface IWeatherApiClient
    {
        Task<string> GetWeatherAsync(string cityName);
    }
}

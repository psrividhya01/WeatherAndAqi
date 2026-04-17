namespace WeatherAPI.DTOs
{
    public class PollutantDto
    {
        public double CurrentValue { get; set; }
        public double WHOlimit { get; set; } = 0;
    }
}

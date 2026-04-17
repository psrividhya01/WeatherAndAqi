namespace WeatherAPI.DTOs
{
    public class AQITrendDto
    {
        public DateTime date { get; set;  } = DateTime.Now;
        public int AQIScore { get; set; }
    }
}

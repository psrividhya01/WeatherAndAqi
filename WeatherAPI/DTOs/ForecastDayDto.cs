namespace WeatherAPI.DTOs
{
    public class ForecastDayDto
    {
        public string Date { get; set; } = string.Empty;
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public string Description { get; set; } = string.Empty;
        public int ConditionCode { get; set; }
        public double PrecipitationProbability { get; set; }
    }
}

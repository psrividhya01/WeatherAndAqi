namespace WeatherAPI.DTOs
{
    public class ForeCastDto
    {
        public DateTime? Date { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int ConditionCode { get; set; }
        public string ConditionDescription { get; set; } = string.Empty;

    }
}

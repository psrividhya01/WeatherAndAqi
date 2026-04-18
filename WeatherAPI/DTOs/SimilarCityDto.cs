namespace WeatherAPI.DTOs
{
    public class SimilarCityDto
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double SimilarityScore { get; set; }
        public string MatchDescription { get; set; } = string.Empty;
    }
}

namespace WeatherAPI.DTOs
{
    public class CitySimilarityDto
    {
        public string CityName { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public string Condition { get; set; } = string.Empty;
        public double SimilarityScore { get; set; }
    }
}

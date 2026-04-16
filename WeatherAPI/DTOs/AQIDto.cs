namespace WeatherAPI.DTOs
{
    public class AQIDto
    {
        public string City { get; set; } = string.Empty;
        public int Aqi { get; set; }
        public string Status { get; set; } = string.Empty;
        public double Co { get; set; }
        public double No2 { get; set; }
        public double O3 { get; set; }
        public double So2 { get; set; }
        public double Pm2_5 { get; set; }
        public double Pm10 { get; set; }
    }
}

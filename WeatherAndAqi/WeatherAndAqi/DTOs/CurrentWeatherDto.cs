namespace WeatherAndAqi.DTOs;
public class CurrentWeatherDto
{
    public string City { get; set; }
    public double Temperature { get; set; }
    public double FeelsLike { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int Visibility { get; set; }       
    public string Condition { get; set; }
    public int ConditionCode { get; set; }     
}
using System.Collections.Generic;

namespace WeatherAndAqi.DTOs;

public class WeatherResponseDto
{ 
    public string City { get; set; }
    public CurrentWeatherDto CurrentWeather { get; set; }
    public  List <HourlyWeatherDto> Hourly { get; set; }
    public List <ForecastDto> Forecast { get; set; }
     public AQIDto AQI { get; set; }
}
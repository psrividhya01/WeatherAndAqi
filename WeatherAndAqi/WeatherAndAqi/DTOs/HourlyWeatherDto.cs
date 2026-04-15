using System;

namespace WeatherAndAqi.DTOs;

public class HourlyWeatherDto
{
    public DateTime Time { get; set; }

    public double Temperature { get; set; }
    public double PrecipitationProbability { get; set; }
}
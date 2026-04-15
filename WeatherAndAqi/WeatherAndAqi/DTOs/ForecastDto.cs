using System;

namespace WeatherAndAqi.DTOs;

public class ForecastDto
{
    
    public string Day { get; set; }
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public string Condition { get; set; }
    public double PrecipitationProbability { get; set; } 
    public string Icon { get; set; }                    
    }

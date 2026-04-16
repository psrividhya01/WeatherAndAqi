using System;

namespace WeatherAPI.DTOs
{
    public class HourlyDataDto
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public double PrecipitationProbability { get; set; }
        public int ConditionCode { get; set; }
    }
}

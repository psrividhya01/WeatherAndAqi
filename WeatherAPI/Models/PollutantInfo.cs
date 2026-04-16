using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class PollutantInfo
    {
        [Key]
        public string PollutantCode { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CommonSources { get; set; } = string.Empty;
    }
}

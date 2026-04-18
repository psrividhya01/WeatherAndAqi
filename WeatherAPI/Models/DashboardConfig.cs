using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class DashboardConfig
    {
        [Key]
        public int UserId { get; set; }
        public string ConfigJson { get; set; } = string.Empty;
    }
}

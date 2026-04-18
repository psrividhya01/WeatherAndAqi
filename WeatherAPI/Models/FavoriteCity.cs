using System;
using System.ComponentModel.DataAnnotations;

namespace WeatherAPI.Models
{
    public class FavoriteCity
    {
        [Key]
        public int FavoriteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string CityName { get; set; } = string.Empty;

        public DateTime SavedAt { get; set; } = DateTime.UtcNow;
    }
}

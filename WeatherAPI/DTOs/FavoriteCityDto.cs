namespace WeatherAPI.DTOs
{
    public class FavoriteCityDto
    {
        public int FavoriteId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteCityRepository _repository;

        public FavoritesController(IFavoriteCityRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetFavorites([FromQuery] int userId)
        {
            var favorites = await _repository.GetFavoritesAsync(userId);
            return Ok(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] FavoriteCity favorite)
        {
            await _repository.AddFavoriteAsync(favorite);
            return Ok("Favorite added");
        }

        [HttpDelete("{cityName}")]
        public async Task<IActionResult> RemoveFavorite([FromQuery] int userId, [FromRoute] string cityName)
        {
            await _repository.RemoveFavoriteAsync(userId, cityName);
            return Ok("Favorite removed");
        }

        [HttpGet("check/{cityName}")]
        public async Task<IActionResult> IsFavorite([FromQuery] int userId, [FromRoute] string cityName)
        {
            var result = await _repository.IsFavoriteAsync(userId, cityName);
            return Ok(result);
        }
    }
}

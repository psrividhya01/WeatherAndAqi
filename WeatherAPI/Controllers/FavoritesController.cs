using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoriteRepository _repository;
        private readonly ILogger<FavoritesController> _logger;

        public FavoritesController(IFavoriteRepository repository, ILogger<FavoritesController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        // GET: api/favorites
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                var userId = GetCurrentUserId();
                var favorites = await _repository.GetFavoritesAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving favorites.");
                return StatusCode(500, "An error occurred while retrieving favorites.");
            }
        }

        // POST: api/favorites
        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            try
            {
                var userId = GetCurrentUserId();

                if (await _repository.ExistsAsync(userId, city))
                    return BadRequest("City is already in favorites.");

                var favorite = new FavoriteCity
                {
                    UserId = userId,
                    CityName = city,
                    SavedAt = DateTime.UtcNow
                };

                await _repository.AddFavoriteAsync(favorite);
                await _repository.SaveChangesAsync();

                return CreatedAtAction(nameof(GetFavorites), new { id = favorite.FavoriteId }, favorite);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding favorite: {City}", city);
                return StatusCode(500, "An error occurred while adding the favorite.");
            }
        }

        // DELETE: api/favorites/{city}
        [HttpDelete("{city}")]
        public async Task<IActionResult> RemoveFavorite(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City name is required.");

            try
            {
                var userId = GetCurrentUserId();
                var favorite = await _repository.GetFavoriteAsync(userId, city);

                if (favorite == null)
                    return NotFound("Favorite not found.");

                await _repository.RemoveFavoriteAsync(favorite);
                await _repository.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing favorite: {City}", city);
                return StatusCode(500, "An error occurred while removing the favorite.");
            }
        }

        private int GetCurrentUserId()
        {
            // In a real application, this would come from User.Claims or HttpContext.User
            // For now, we return a hardcoded ID, but structured to be easily replaced by Identity logic.
            return 1; 
        }
    }
}

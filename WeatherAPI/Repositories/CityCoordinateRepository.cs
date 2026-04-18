using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeatherAPI.Data;
using WeatherAPI.Interfaces;
using WeatherAPI.Models;

namespace WeatherAPI.Repositories
{
    public class CityCoordinateRepository : ICityCoordinateRepository
    {
        private readonly AppDbContext _context;

        public CityCoordinateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CityCoordinate>> GetNearbyCoordinatesAsync(double lat, double lon, int maxResults = 8)
        {
            var coordinates = await _context.Set<CityCoordinate>().ToListAsync();

            if (!coordinates.Any())
            {
                return new List<CityCoordinate>();
            }

            return coordinates
                .Select(c => new {
                    City = c,
                    Distance = HaversineDistance(lat, lon, c.Latitude, c.Longitude)
                })
                .OrderBy(x => x.Distance)
                .Take(maxResults)
                .Select(x => x.City)
                .ToList();
        }

        public async Task<List<CityCoordinate>> GetAllCoordinatesAsync()
        {
            return await _context.CityCoordinates.ToListAsync();
        }

        private static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // kilometers
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double ToRadians(double degrees) => degrees * Math.PI / 180.0;
    }
}

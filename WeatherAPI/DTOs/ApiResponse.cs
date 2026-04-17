using System;

namespace WeatherAPI.DTOs
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool Success { get; set; } = true;
        public string? ErrorMessage { get; set; }

        public static ApiResponse<T> Ok(T data) => new ApiResponse<T> { Data = data };
        public static ApiResponse<T> Fail(string message) => new ApiResponse<T> { Success = false, ErrorMessage = message };
    }
}

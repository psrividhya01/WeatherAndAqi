using WeatherAPI.Data;
using WeatherAPI.ExternalServices;
using WeatherAPI.Repositories;
using WeatherAPI.Services;
using WeatherAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register Dependencies
builder.Services.AddSingleton<SqlDbConnectionFactory>();

// Repositories
builder.Services.AddScoped<IWeatherCacheRepository, WeatherCacheRepository>();
builder.Services.AddScoped<IHourlyCacheRepository, HourlyCacheRepository>();
builder.Services.AddScoped<IForecastCacheRepository, ForecastCacheRepository>();

// External API
builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();

// Services
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
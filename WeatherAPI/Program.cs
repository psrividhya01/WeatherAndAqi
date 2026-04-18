using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.ExternalServices;
using WeatherAPI.Repositories;
using WeatherAPI.Services;
using WeatherAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOpenApi();

// ─── CORS — Allow Angular frontend to call this API ───────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",   // Angular dev server
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// ─── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─── Repositories ─────────────────────────────────────────────────────────────
builder.Services.AddScoped<IWeatherCacheRepository, WeatherCacheRepository>();
builder.Services.AddScoped<IHourlyCacheRepository, HourlyCacheRepository>();
builder.Services.AddScoped<IForecastCacheRepository, ForecastCacheRepository>();
builder.Services.AddScoped<IAQICacheRepository, AQICacheRepository>();
builder.Services.AddScoped<ICityCoordinateRepository, CityCoordinateRepository>();
builder.Services.AddScoped<ICachedCityWeatherRepository, CachedCityWeatherRepository>();
builder.Services.AddScoped<IAQIHistoryRepository, AQIHistoryRepository>();
builder.Services.AddScoped<IWeatherAlertRepository, WeatherAlertRepository>();
builder.Services.AddScoped<IAQIAdvisoryRepository, AQIAdvisoryRepository>();
builder.Services.AddScoped<IFavoriteCityRepository, FavoriteCityRepository>();
builder.Services.AddScoped<IUserPreferenceRepository, UserPreferenceRepository>();

// ─── External API Clients ─────────────────────────────────────────────────────
builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();
builder.Services.AddHttpClient<IAQIApiClient, AQIApiClient>();

// ─── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IAQIService, AQIService>();

var app = builder.Build();

// ─── Middleware Pipeline ───────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();

// CORS must come BEFORE UseAuthorization and MapControllers
app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();
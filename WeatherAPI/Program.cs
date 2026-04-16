using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.ExternalServices;
using WeatherAPI.Repositories;
using WeatherAPI.Services;
using WeatherAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
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

// ─── External API Client ───────────────────────────────────────────────────────
builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();

// ─── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();

var app = builder.Build();

// ─── Middleware Pipeline ───────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// CORS must come BEFORE UseAuthorization and MapControllers
app.UseCors("AllowAngularDev");

app.MapControllers();

app.Run();
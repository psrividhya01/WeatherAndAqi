using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.ExternalServices;
using WeatherAPI.Repositories;
using WeatherAPI.Services;
using WeatherAPI.Interfaces;
using WeatherAPI.BackgroundServices;
using WeatherAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ─── 1. SERVICES ─────────────────────────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader().AllowAnyMethod();
    });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<IWeatherCacheRepository, WeatherCacheRepository>();
builder.Services.AddScoped<IHourlyCacheRepository, HourlyCacheRepository>();
builder.Services.AddScoped<IForecastCacheRepository, ForecastCacheRepository>();
builder.Services.AddScoped<IAQICacheRepository, AQICacheRepository>();

// External API Clients
builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>();

// Services
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IForecastService, ForecastService>();
builder.Services.AddScoped<IAQIService, AQIService>();

// Background Jobs (Module 3 Task 3)
builder.Services.AddHostedService<AQIBackgroundService>();

var app = builder.Build();

// ─── 2. MIDDLEWARE & STARTUP ──────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Global Exception Middleware (Module 3 Task 6)
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
app.MapControllers();

// ─── 3. SEEDING (Module 3 Task 4) ──────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbInitializer.Seed(context);
}

app.Run();
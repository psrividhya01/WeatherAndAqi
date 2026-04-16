using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForecastCaches",
                columns: table => new
                {
                    CacheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForecastJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForecastCaches", x => x.CacheId);
                });

            migrationBuilder.CreateTable(
                name: "HourlyCaches",
                columns: table => new
                {
                    CacheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyCaches", x => x.CacheId);
                });

            migrationBuilder.CreateTable(
                name: "HourlyWeatherCaches",
                columns: table => new
                {
                    CacheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HourlyJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HourlyWeatherCaches", x => x.CacheId);
                });

            migrationBuilder.CreateTable(
                name: "WeatherCaches",
                columns: table => new
                {
                    CacheId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherCaches", x => x.CacheId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForecastCaches");

            migrationBuilder.DropTable(
                name: "HourlyCaches");

            migrationBuilder.DropTable(
                name: "HourlyWeatherCaches");

            migrationBuilder.DropTable(
                name: "WeatherCaches");
        }
    }
}

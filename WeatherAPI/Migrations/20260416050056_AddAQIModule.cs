using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddAQIModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AQICategory",
                table: "AQICaches",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AQIScore",
                table: "AQICaches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AQIAdvisories",
                columns: table => new
                {
                    AdvisoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Advisory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensitiveGroupNote = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AQIAdvisories", x => x.AdvisoryId);
                });

            migrationBuilder.CreateTable(
                name: "AQIHistories",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AQIScore = table.Column<int>(type: "int", nullable: false),
                    RecordedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AQIHistories", x => x.HistoryId);
                });

            migrationBuilder.CreateTable(
                name: "PollutantInfos",
                columns: table => new
                {
                    PollutantCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommonSources = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollutantInfos", x => x.PollutantCode);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AQIAdvisories");

            migrationBuilder.DropTable(
                name: "AQIHistories");

            migrationBuilder.DropTable(
                name: "PollutantInfos");

            migrationBuilder.DropColumn(
                name: "AQICategory",
                table: "AQICaches");

            migrationBuilder.DropColumn(
                name: "AQIScore",
                table: "AQICaches");
        }
    }
}

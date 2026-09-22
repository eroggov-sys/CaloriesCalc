using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceEatenAtWithDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodEntries_UserId",
                table: "FoodEntries");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Date",
                table: "FoodEntries",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.Sql("UPDATE \"FoodEntries\" SET \"Date\" = \"EatenAt\"::date;");

            migrationBuilder.DropColumn(
                name: "EatenAt",
                table: "FoodEntries");

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_UserId_Date",
                table: "FoodEntries",
                columns: new[] { "UserId", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FoodEntries_UserId_Date",
                table: "FoodEntries");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "FoodEntries");

            migrationBuilder.AddColumn<DateTime>(
                name: "EatenAt",
                table: "FoodEntries",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_UserId",
                table: "FoodEntries",
                column: "UserId");
        }
    }
}

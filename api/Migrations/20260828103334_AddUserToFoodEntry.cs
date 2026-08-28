using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToFoodEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "FoodEntries",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FoodEntries_UserId",
                table: "FoodEntries",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries");

            migrationBuilder.DropIndex(
                name: "IX_FoodEntries_UserId",
                table: "FoodEntries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "FoodEntries");
        }
    }
}

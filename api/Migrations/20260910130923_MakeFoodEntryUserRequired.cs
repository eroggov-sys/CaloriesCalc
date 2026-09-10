using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class MakeFoodEntryUserRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"FoodEntries\" WHERE \"UserId\" IS NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FoodEntries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "FoodEntries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodEntries_AspNetUsers_UserId",
                table: "FoodEntries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

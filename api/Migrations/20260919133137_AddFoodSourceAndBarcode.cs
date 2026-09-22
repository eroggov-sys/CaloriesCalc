using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodSourceAndBarcode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Foods",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Foods",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Foods",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Foods",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Foods",
                type: "text",
                nullable: false,
                defaultValue: "Manual");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Barcode",
                table: "Foods",
                column: "Barcode",
                filter: "\"Barcode\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Foods_Source_ExternalId",
                table: "Foods",
                columns: new[] { "Source", "ExternalId" },
                unique: true,
                filter: "\"ExternalId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Foods_Barcode",
                table: "Foods");

            migrationBuilder.DropIndex(
                name: "IX_Foods_Source_ExternalId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Foods");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Foods");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Foods",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}

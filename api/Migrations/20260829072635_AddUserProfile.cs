using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WeightKg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: false),
                    HeightCm = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    BiologicalSex = table.Column<int>(type: "integer", nullable: false),
                    ActivityLevel = table.Column<int>(type: "integer", nullable: false),
                    NutritionGoal = table.Column<int>(type: "integer", nullable: false),
                    BodyFatPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.CheckConstraint("CK_UserProfile_WeightKg", "\"WeightKg\" >= 20 AND \"WeightKg\" <= 500");
                    table.CheckConstraint("CK_UserProfiles_BodyFatPercentage", "\"BodyFatPercentage\" IS NULL OR (\"BodyFatPercentage\" >= 1 AND \"BodyFatPercentage\" <= 75)");
                    table.CheckConstraint("CK_UserProfiles_HeightCm", "\"HeightCm\" >= 50 AND \"HeightCm\" <= 300");
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_UserId",
                table: "UserProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProfiles");
        }
    }
}

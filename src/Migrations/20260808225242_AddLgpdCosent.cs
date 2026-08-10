using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NordesteFoodAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLgpdCosent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LgpdConsentGiven",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LgpdConsentGiven",
                table: "AspNetUsers");
        }
    }
}

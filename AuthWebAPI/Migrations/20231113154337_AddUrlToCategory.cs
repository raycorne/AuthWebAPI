using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppWebAPI.Migrations
{
    public partial class AddUrlToCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "FurnitureCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "FurnitureCategories");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppWebAPI.Migrations
{
    public partial class EditingCategory1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDisplayedInNavbar",
                table: "FurnitureCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisplayedInNavbar",
                table: "FurnitureCategories");
        }
    }
}

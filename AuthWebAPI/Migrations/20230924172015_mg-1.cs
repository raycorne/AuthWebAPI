using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppWebAPI.Migrations
{
    public partial class mg1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Furnitures");

            migrationBuilder.RenameColumn(
                name: "ImgUrl",
                table: "Furnitures",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "Furnitures",
                newName: "Price");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Furnitures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FurnitureCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureCategories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Furnitures_CategoryId",
                table: "Furnitures",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Furnitures_FurnitureCategories_CategoryId",
                table: "Furnitures",
                column: "CategoryId",
                principalTable: "FurnitureCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Furnitures_FurnitureCategories_CategoryId",
                table: "Furnitures");

            migrationBuilder.DropTable(
                name: "FurnitureCategories");

            migrationBuilder.DropIndex(
                name: "IX_Furnitures_CategoryId",
                table: "Furnitures");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Furnitures");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Furnitures",
                newName: "Cost");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Furnitures",
                newName: "ImgUrl");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Furnitures",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}

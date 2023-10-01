using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppWebAPI.Migrations
{
    public partial class mgimagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Furnitures");

            migrationBuilder.CreateTable(
                name: "FurnitureImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Uri = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FurnitureId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurnitureImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FurnitureImages_Furnitures_FurnitureId",
                        column: x => x.FurnitureId,
                        principalTable: "Furnitures",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FurnitureImages_FurnitureId",
                table: "FurnitureImages",
                column: "FurnitureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FurnitureImages");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Furnitures",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobileAppWebAPI.Migrations
{
    public partial class imagetable3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FurnitureImages_Furnitures_FurnitureId",
                table: "FurnitureImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "FurnitureId",
                table: "FurnitureImages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FurnitureImages_Furnitures_FurnitureId",
                table: "FurnitureImages",
                column: "FurnitureId",
                principalTable: "Furnitures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FurnitureImages_Furnitures_FurnitureId",
                table: "FurnitureImages");

            migrationBuilder.AlterColumn<Guid>(
                name: "FurnitureId",
                table: "FurnitureImages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_FurnitureImages_Furnitures_FurnitureId",
                table: "FurnitureImages",
                column: "FurnitureId",
                principalTable: "Furnitures",
                principalColumn: "Id");
        }
    }
}

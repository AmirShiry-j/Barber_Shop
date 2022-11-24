using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Add_Owner_colomn_for_Salon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Salons",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "SalonId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SalonId",
                table: "AspNetUsers",
                column: "SalonId",
                unique: true,
                filter: "[SalonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Salons_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SalonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Salons");

            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class ref_Barber_Salon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barbers_Salons_SalonId",
                table: "Barbers");

            migrationBuilder.AddForeignKey(
                name: "FK_Barbers_Salons_SalonId",
                table: "Barbers",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Barbers_Salons_SalonId",
                table: "Barbers");

            migrationBuilder.AddForeignKey(
                name: "FK_Barbers_Salons_SalonId",
                table: "Barbers",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id");
        }
    }
}

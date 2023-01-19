using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class remove_SalonId_from_address_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_addresses_Salons_SalonId",
                table: "addresses");

            migrationBuilder.DropIndex(
                name: "IX_addresses_SalonId",
                table: "addresses");

            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Salons_AddressId",
                table: "Salons",
                column: "AddressId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Salons_addresses_AddressId",
                table: "Salons",
                column: "AddressId",
                principalTable: "addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Salons_addresses_AddressId",
                table: "Salons");

            migrationBuilder.DropIndex(
                name: "IX_Salons_AddressId",
                table: "Salons");

            migrationBuilder.AddColumn<int>(
                name: "SalonId",
                table: "addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_addresses_SalonId",
                table: "addresses",
                column: "SalonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_addresses_Salons_SalonId",
                table: "addresses",
                column: "SalonId",
                principalTable: "Salons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class update_relations_Address_City : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_addresses_CityId",
                table: "addresses");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CityId",
                table: "addresses",
                column: "CityId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_addresses_CityId",
                table: "addresses");

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CityId",
                table: "addresses",
                column: "CityId",
                unique: true);
        }
    }
}

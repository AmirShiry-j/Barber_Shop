using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class For_Address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Salons");

            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Salons",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Uniteds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uniteds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UnitedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Uniteds_UnitedId",
                        column: x => x.UnitedId,
                        principalTable: "Uniteds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    SalonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_addresses_Salons_SalonId",
                        column: x => x.SalonId,
                        principalTable: "Salons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_addresses_CityId",
                table: "addresses",
                column: "CityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_addresses_SalonId",
                table: "addresses",
                column: "SalonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cities_UnitedId",
                table: "Cities",
                column: "UnitedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "addresses");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Uniteds");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Salons");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Salons",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}

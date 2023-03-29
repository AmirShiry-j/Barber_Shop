using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_TimeModes_and_TimeModeItems_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeModes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeCreate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    TimeLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BarberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeModes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeModes_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TimeModeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hour = table.Column<int>(type: "int", maxLength: 23, nullable: false),
                    Minute = table.Column<int>(type: "int", maxLength: 59, nullable: false),
                    TimeCreate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    TimeLastUpdate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TimeModeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeModeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeModeItems_TimeModes_TimeModeId",
                        column: x => x.TimeModeId,
                        principalTable: "TimeModes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeModeItems_TimeModeId",
                table: "TimeModeItems",
                column: "TimeModeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeModes_BarberId",
                table: "TimeModes",
                column: "BarberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeModeItems");

            migrationBuilder.DropTable(
                name: "TimeModes");
        }
    }
}

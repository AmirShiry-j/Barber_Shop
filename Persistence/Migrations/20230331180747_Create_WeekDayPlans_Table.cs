using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Create_WeekDayPlans_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeekDayPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BarberId = table.Column<int>(type: "int", nullable: false),
                    TimeModeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeekDayPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeekDayPlans_Barbers_BarberId",
                        column: x => x.BarberId,
                        principalTable: "Barbers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeekDayPlans_TimeModes_TimeModeId",
                        column: x => x.TimeModeId,
                        principalTable: "TimeModes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeekDayPlans_BarberId",
                table: "WeekDayPlans",
                column: "BarberId");

            migrationBuilder.CreateIndex(
                name: "IX_WeekDayPlans_TimeModeId",
                table: "WeekDayPlans",
                column: "TimeModeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeekDayPlans");
        }
    }
}

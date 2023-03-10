using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class Add_base_coloumns_to_Main_enities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "Salons",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "Barbers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "TimeCreate",
                table: "AspNetRoles",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "Salons");

            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "Barbers");

            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TimeCreate",
                table: "AspNetRoles");
        }
    }
}

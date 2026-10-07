using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainDentalla.Migrations
{
    public partial class m16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "CalendarDay");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "CalendarDay",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "CalendarDay");

            migrationBuilder.AddColumn<int>(
                name: "DayOfWeek",
                table: "CalendarDay",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

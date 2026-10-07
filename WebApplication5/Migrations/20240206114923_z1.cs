using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainDentalla.Migrations
{
    public partial class z1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Doctors",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "PhotoContentType",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PhotoContentType",
                table: "Doctors");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MainDentalla.Migrations
{
    public partial class m9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_AspNetUsers_DoctorId1",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_DoctorId1",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DoctorId1",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_DoctorId",
                table: "Articles",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Doctors_DoctorId",
                table: "Articles",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Doctors_DoctorId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_DoctorId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "DoctorId1",
                table: "Articles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_DoctorId1",
                table: "Articles",
                column: "DoctorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_AspNetUsers_DoctorId1",
                table: "Articles",
                column: "DoctorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

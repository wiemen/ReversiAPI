using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiRestApi.Migrations
{
    public partial class RemoveNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WinnaarToken",
                table: "Uitslag",
                newName: "Speler2Token");

            migrationBuilder.RenameColumn(
                name: "WinnaarPunten",
                table: "Uitslag",
                newName: "PuntenZwart");

            migrationBuilder.RenameColumn(
                name: "VerliezerToken",
                table: "Uitslag",
                newName: "Speler1Token");

            migrationBuilder.RenameColumn(
                name: "VerliezerPunten",
                table: "Uitslag",
                newName: "PuntenWit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Speler2Token",
                table: "Uitslag",
                newName: "WinnaarToken");

            migrationBuilder.RenameColumn(
                name: "Speler1Token",
                table: "Uitslag",
                newName: "VerliezerToken");

            migrationBuilder.RenameColumn(
                name: "PuntenZwart",
                table: "Uitslag",
                newName: "WinnaarPunten");

            migrationBuilder.RenameColumn(
                name: "PuntenWit",
                table: "Uitslag",
                newName: "VerliezerPunten");
        }
    }
}

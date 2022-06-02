using Microsoft.EntityFrameworkCore.Migrations;

namespace ReversiRestApi.Migrations
{
    public partial class AddedPOints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VerliezerPunten",
                table: "Uitslag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VerliezerToken",
                table: "Uitslag",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WinnaarPunten",
                table: "Uitslag",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerliezerPunten",
                table: "Uitslag");

            migrationBuilder.DropColumn(
                name: "VerliezerToken",
                table: "Uitslag");

            migrationBuilder.DropColumn(
                name: "WinnaarPunten",
                table: "Uitslag");
        }
    }
}

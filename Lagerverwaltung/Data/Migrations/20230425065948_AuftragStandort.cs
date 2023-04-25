using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagerverwaltung.Data.Migrations
{
    public partial class AuftragStandort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StandortID",
                table: "Auftrag",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StandortID",
                table: "Auftrag");
        }
    }
}

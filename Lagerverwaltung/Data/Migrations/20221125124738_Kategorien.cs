using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagerverwaltung.Data.Migrations
{
    public partial class Kategorien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Kategorie",
                table: "Artikel",
                newName: "KategorieID");

            migrationBuilder.CreateTable(
                name: "Kategorien",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Farbe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bemerkungen = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategorien", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artikel_KategorieID",
                table: "Artikel",
                column: "KategorieID");

            migrationBuilder.AddForeignKey(
                name: "FK_Artikel_Kategorien_KategorieID",
                table: "Artikel",
                column: "KategorieID",
                principalTable: "Kategorien",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artikel_Kategorien_KategorieID",
                table: "Artikel");

            migrationBuilder.DropTable(
                name: "Kategorien");

            migrationBuilder.DropIndex(
                name: "IX_Artikel_KategorieID",
                table: "Artikel");

            migrationBuilder.RenameColumn(
                name: "KategorieID",
                table: "Artikel",
                newName: "Kategorie");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagerverwaltung.Data.Migrations
{
    public partial class moreTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Beschreibung",
                table: "Artikel",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Kunde",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vorname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Firma = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PLZ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ansprechpartner = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bemerkungen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kunde", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lagerort",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lagerort", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Auftrag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundeID = table.Column<int>(type: "int", nullable: false),
                    ArtikelID = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<int>(type: "int", nullable: false),
                    Bemerkungen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auftrag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Auftrag_Artikel_ArtikelID",
                        column: x => x.ArtikelID,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Auftrag_Kunde_KundeID",
                        column: x => x.KundeID,
                        principalTable: "Kunde",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lagerplatz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LagerortID = table.Column<int>(type: "int", nullable: false),
                    ArtikelID = table.Column<int>(type: "int", nullable: false),
                    Soll = table.Column<int>(type: "int", nullable: false),
                    Ist = table.Column<int>(type: "int", nullable: false),
                    Bemerkungen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lagerplatz", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lagerplatz_Artikel_ArtikelID",
                        column: x => x.ArtikelID,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lagerplatz_Lagerort_LagerortID",
                        column: x => x.LagerortID,
                        principalTable: "Lagerort",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Auftrag_ArtikelID",
                table: "Auftrag",
                column: "ArtikelID");

            migrationBuilder.CreateIndex(
                name: "IX_Auftrag_KundeID",
                table: "Auftrag",
                column: "KundeID");

            migrationBuilder.CreateIndex(
                name: "IX_Lagerplatz_ArtikelID",
                table: "Lagerplatz",
                column: "ArtikelID");

            migrationBuilder.CreateIndex(
                name: "IX_Lagerplatz_LagerortID",
                table: "Lagerplatz",
                column: "LagerortID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auftrag");

            migrationBuilder.DropTable(
                name: "Lagerplatz");

            migrationBuilder.DropTable(
                name: "Kunde");

            migrationBuilder.DropTable(
                name: "Lagerort");

            migrationBuilder.AlterColumn<string>(
                name: "Beschreibung",
                table: "Artikel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagerverwaltung.Data.Migrations
{
    public partial class positionen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auftrag_Artikel_ArtikelID",
                table: "Auftrag");

            migrationBuilder.DropIndex(
                name: "IX_Auftrag_ArtikelID",
                table: "Auftrag");

            migrationBuilder.DropColumn(
                name: "ArtikelID",
                table: "Auftrag");

            migrationBuilder.DropColumn(
                name: "Menge",
                table: "Auftrag");

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuftragsID = table.Column<int>(type: "int", nullable: false),
                    PositionsNummer = table.Column<int>(type: "int", nullable: false),
                    ArtikelID = table.Column<int>(type: "int", nullable: false),
                    Menge = table.Column<int>(type: "int", nullable: false),
                    Bemerkungen = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Position_Artikel_ArtikelID",
                        column: x => x.ArtikelID,
                        principalTable: "Artikel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Position_Auftrag_AuftragsID",
                        column: x => x.AuftragsID,
                        principalTable: "Auftrag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Position_ArtikelID",
                table: "Position",
                column: "ArtikelID");

            migrationBuilder.CreateIndex(
                name: "IX_Position_AuftragsID",
                table: "Position",
                column: "AuftragsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.AddColumn<int>(
                name: "ArtikelID",
                table: "Auftrag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Menge",
                table: "Auftrag",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Auftrag_ArtikelID",
                table: "Auftrag",
                column: "ArtikelID");

            migrationBuilder.AddForeignKey(
                name: "FK_Auftrag_Artikel_ArtikelID",
                table: "Auftrag",
                column: "ArtikelID",
                principalTable: "Artikel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

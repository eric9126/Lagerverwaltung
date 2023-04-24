using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lagerverwaltung.Data.Migrations
{
    public partial class Standort : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Standort",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StandortId = table.Column<int>(type: "int", nullable: false),
                    KundeID = table.Column<int>(type: "int", nullable: false),
                    PLZ = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Land = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bundesland = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Strasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ansprechpartner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Eintragungsdatum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Schliessungsdatum = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Standortbezeichnung = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Standort", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Standort_Kunde_KundeID",
                        column: x => x.KundeID,
                        principalTable: "Kunde",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Standort_KundeID",
                table: "Standort",
                column: "KundeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Standort");
        }
    }
}

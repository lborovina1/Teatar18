using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teatar18_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrvaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "brojKupljenihKarata",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "newsletter",
                table: "AspNetUsers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Newsletter",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    informacija = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datumSlanja = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Newsletter", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pitanje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKorisnikaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    predmet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sadrzaj = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    datumPostavljanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    odgovoreno = table.Column<bool>(type: "bit", nullable: false),
                    datumOdgovora = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IDZaposlenikaId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pitanje", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pitanje_AspNetUsers_IDKorisnikaId",
                        column: x => x.IDKorisnikaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pitanje_AspNetUsers_IDZaposlenikaId",
                        column: x => x.IDZaposlenikaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Predstava",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    glumci = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    scenaristi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    reziseri = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    scenografi = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    zanr = table.Column<int>(type: "int", nullable: false),
                    opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    poster = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    trajanje = table.Column<int>(type: "int", nullable: false),
                    uRepertoaru = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predstava", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacija",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDKorisnikaId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    kupovina = table.Column<bool>(type: "bit", nullable: false),
                    brojKarata = table.Column<int>(type: "int", nullable: false),
                    popust = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacija", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                        column: x => x.IDKorisnikaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Izvedba",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPredstave = table.Column<int>(type: "int", nullable: false),
                    vrijeme = table.Column<DateTime>(type: "datetime2", nullable: false),
                    brojSlobodnihKarata = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Izvedba", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Izvedba_Predstava_IDPredstave",
                        column: x => x.IDPredstave,
                        principalTable: "Predstava",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocjena",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDPredstave = table.Column<int>(type: "int", nullable: false),
                    ocjena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocjena", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ocjena_Predstava_IDPredstave",
                        column: x => x.IDPredstave,
                        principalTable: "Predstava",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Karta",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDIzvedbe = table.Column<int>(type: "int", nullable: false),
                    sjediste = table.Column<int>(type: "int", nullable: false),
                    cijena = table.Column<double>(type: "float", nullable: false),
                    placena = table.Column<bool>(type: "bit", nullable: false),
                    IDRezervacije = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Karta", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Karta_Izvedba_IDIzvedbe",
                        column: x => x.IDIzvedbe,
                        principalTable: "Izvedba",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Karta_Rezervacija_IDRezervacije",
                        column: x => x.IDRezervacije,
                        principalTable: "Rezervacija",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Izvedba_IDPredstave",
                table: "Izvedba",
                column: "IDPredstave");

            migrationBuilder.CreateIndex(
                name: "IX_Karta_IDIzvedbe",
                table: "Karta",
                column: "IDIzvedbe");

            migrationBuilder.CreateIndex(
                name: "IX_Karta_IDRezervacije",
                table: "Karta",
                column: "IDRezervacije");

            migrationBuilder.CreateIndex(
                name: "IX_Ocjena_IDPredstave",
                table: "Ocjena",
                column: "IDPredstave");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanje_IDKorisnikaId",
                table: "Pitanje",
                column: "IDKorisnikaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pitanje_IDZaposlenikaId",
                table: "Pitanje",
                column: "IDZaposlenikaId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IDKorisnikaId",
                table: "Rezervacija",
                column: "IDKorisnikaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Karta");

            migrationBuilder.DropTable(
                name: "Newsletter");

            migrationBuilder.DropTable(
                name: "Ocjena");

            migrationBuilder.DropTable(
                name: "Pitanje");

            migrationBuilder.DropTable(
                name: "Izvedba");

            migrationBuilder.DropTable(
                name: "Rezervacija");

            migrationBuilder.DropTable(
                name: "Predstava");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "brojKupljenihKarata",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "newsletter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "prezime",
                table: "AspNetUsers");
        }
    }
}

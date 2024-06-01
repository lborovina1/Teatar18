using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teatar18_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class IzmjenaRezervacijeMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IDIzvedbe",
                table: "Rezervacija",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacija_IDIzvedbe",
                table: "Rezervacija",
                column: "IDIzvedbe");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_Izvedba_IDIzvedbe",
                table: "Rezervacija",
                column: "IDIzvedbe",
                principalTable: "Izvedba",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_Izvedba_IDIzvedbe",
                table: "Rezervacija");

            migrationBuilder.DropIndex(
                name: "IX_Rezervacija_IDIzvedbe",
                table: "Rezervacija");

            migrationBuilder.DropColumn(
                name: "IDIzvedbe",
                table: "Rezervacija");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teatar18_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class NullableKorisnikMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDKorisnikaId",
                table: "Pitanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDZaposlenikaId",
                table: "Pitanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<string>(
                name: "IDKorisnikaId",
                table: "Rezervacija",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IDZaposlenikaId",
                table: "Pitanje",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "IDKorisnikaId",
                table: "Pitanje",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDKorisnikaId",
                table: "Pitanje",
                column: "IDKorisnikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDZaposlenikaId",
                table: "Pitanje",
                column: "IDZaposlenikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                table: "Rezervacija",
                column: "IDKorisnikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDKorisnikaId",
                table: "Pitanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDZaposlenikaId",
                table: "Pitanje");

            migrationBuilder.DropForeignKey(
                name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                table: "Rezervacija");

            migrationBuilder.AlterColumn<string>(
                name: "IDKorisnikaId",
                table: "Rezervacija",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IDZaposlenikaId",
                table: "Pitanje",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IDKorisnikaId",
                table: "Pitanje",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDKorisnikaId",
                table: "Pitanje",
                column: "IDKorisnikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pitanje_AspNetUsers_IDZaposlenikaId",
                table: "Pitanje",
                column: "IDZaposlenikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                table: "Rezervacija",
                column: "IDKorisnikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teatar18_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class customFields : Migration
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

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

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

            migrationBuilder.AlterColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "newsletter",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "brojKupljenihKarata",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
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
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Rezervacija_AspNetUsers_IDKorisnikaId",
                table: "Rezervacija",
                column: "IDKorisnikaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.AlterColumn<string>(
                name: "prezime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "newsletter",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "ime",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "brojKupljenihKarata",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

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
    }
}

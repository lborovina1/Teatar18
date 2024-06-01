using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Teatar18_2.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigracijaNakonIzmjenePredstave : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "preporucena",
                table: "Predstava",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "preporucena",
                table: "Predstava");
        }
    }
}

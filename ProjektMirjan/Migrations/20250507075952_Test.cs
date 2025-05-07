using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektMirjan.Migrations
{
    /// <inheritdoc />
    public partial class Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "CurrencyRates");

            migrationBuilder.DropColumn(
                name: "Symbol",
                table: "CurrencyRates");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CurrencyRates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3)",
                oldMaxLength: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "CurrencyRates",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "CurrencyRates",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Symbol",
                table: "CurrencyRates",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

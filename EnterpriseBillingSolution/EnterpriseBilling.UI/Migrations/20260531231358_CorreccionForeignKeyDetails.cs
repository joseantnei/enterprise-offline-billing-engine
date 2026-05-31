using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseBilling.UI.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionForeignKeyDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "PercentTaxes",
                table: "BillDetails",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "TaxesTypes",
                keyColumn: "IdTaxesType",
                keyValue: 3,
                column: "Percent",
                value: 13m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PercentTaxes",
                table: "BillDetails",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "TaxesTypes",
                keyColumn: "IdTaxesType",
                keyValue: 3,
                column: "Percent",
                value: 18m);
        }
    }
}

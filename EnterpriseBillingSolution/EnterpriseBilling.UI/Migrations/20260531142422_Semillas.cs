using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EnterpriseBilling.UI.Migrations
{
    /// <inheritdoc />
    public partial class Semillas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SalePrice",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                table: "Products",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "REAL");

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Email", "Name", "Phone" },
                values: new object[] { 1, "xxx@xxx.com", "end consumer", "000000000" });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "IdProduct", "Barcode", "Cost", "NameProduct", "SalePrice", "Stock" },
                values: new object[,]
                {
                    { 1, "101010", 20m, "KeyBoard RGB", 24.50m, 20 },
                    { 2, "202020", 10m, "Mouse RGB", 14.50m, 20 },
                    { 3, "303030", 11m, "Windows licences", 12.8m, 20 }
                });

            migrationBuilder.InsertData(
                table: "TaxesTypes",
                columns: new[] { "IdTaxesType", "NameTaxes", "Percent" },
                values: new object[,]
                {
                    { 2, "GST/IVA (18%)", 18m },
                    { 3, "IVA (13%)", 18m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TaxesTypes",
                keyColumn: "IdTaxesType",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TaxesTypes",
                keyColumn: "IdTaxesType",
                keyValue: 3);

            migrationBuilder.AlterColumn<float>(
                name: "SalePrice",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<float>(
                name: "Cost",
                table: "Products",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");
        }
    }
}

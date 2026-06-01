using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseBilling.UI.Migrations
{
    /// <inheritdoc />
    public partial class FixBillDetailRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bills_BillIdBill",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Products_ProductIdProduct",
                table: "BillDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillDetails_BillIdBill",
                table: "BillDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillDetails_ProductIdProduct",
                table: "BillDetails");

            migrationBuilder.DropColumn(
                name: "BillIdBill",
                table: "BillDetails");

            migrationBuilder.DropColumn(
                name: "ProductIdProduct",
                table: "BillDetails");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_IdBill",
                table: "BillDetails",
                column: "IdBill");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_IdProduct",
                table: "BillDetails",
                column: "IdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bills_IdBill",
                table: "BillDetails",
                column: "IdBill",
                principalTable: "Bills",
                principalColumn: "IdBill",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Products_IdProduct",
                table: "BillDetails",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "IdProduct",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Bills_IdBill",
                table: "BillDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_BillDetails_Products_IdProduct",
                table: "BillDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillDetails_IdBill",
                table: "BillDetails");

            migrationBuilder.DropIndex(
                name: "IX_BillDetails_IdProduct",
                table: "BillDetails");

            migrationBuilder.AddColumn<int>(
                name: "BillIdBill",
                table: "BillDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductIdProduct",
                table: "BillDetails",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_BillIdBill",
                table: "BillDetails",
                column: "BillIdBill");

            migrationBuilder.CreateIndex(
                name: "IX_BillDetails_ProductIdProduct",
                table: "BillDetails",
                column: "ProductIdProduct");

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Bills_BillIdBill",
                table: "BillDetails",
                column: "BillIdBill",
                principalTable: "Bills",
                principalColumn: "IdBill",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BillDetails_Products_ProductIdProduct",
                table: "BillDetails",
                column: "ProductIdProduct",
                principalTable: "Products",
                principalColumn: "IdProduct",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

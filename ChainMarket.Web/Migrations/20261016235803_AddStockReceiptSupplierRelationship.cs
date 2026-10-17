using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStockReceiptSupplierRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_SupplierId",
                table: "StockReceipts",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockReceipts_Suppliers_SupplierId",
                table: "StockReceipts",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockReceipts_Suppliers_SupplierId",
                table: "StockReceipts");

            migrationBuilder.DropIndex(
                name: "IX_StockReceipts_SupplierId",
                table: "StockReceipts");
        }
    }
}

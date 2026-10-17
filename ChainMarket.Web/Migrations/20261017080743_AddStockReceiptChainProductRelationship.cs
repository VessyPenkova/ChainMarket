using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddStockReceiptChainProductRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockReceipts_ChainProductId",
                table: "StockReceipts",
                column: "ChainProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockReceipts_ChainProducts_ChainProductId",
                table: "StockReceipts",
                column: "ChainProductId",
                principalTable: "ChainProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockReceipts_ChainProducts_ChainProductId",
                table: "StockReceipts");

            migrationBuilder.DropIndex(
                name: "IX_StockReceipts_ChainProductId",
                table: "StockReceipts");
        }
    }
}

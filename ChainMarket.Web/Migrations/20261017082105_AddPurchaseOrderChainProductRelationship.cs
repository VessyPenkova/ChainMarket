using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseOrderChainProductRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_ChainProductId",
                table: "PurchaseOrders",
                column: "ChainProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_ChainProducts_ChainProductId",
                table: "PurchaseOrders",
                column: "ChainProductId",
                principalTable: "ChainProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_ChainProducts_ChainProductId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_ChainProductId",
                table: "PurchaseOrders");
        }
    }
}

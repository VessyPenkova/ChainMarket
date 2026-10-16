using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChainProductCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChainProducts_CategoryId",
                table: "ChainProducts",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts");

            migrationBuilder.DropIndex(
                name: "IX_ChainProducts_CategoryId",
                table: "ChainProducts");
        }
    }
}

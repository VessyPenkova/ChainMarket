using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class RestrictCategoryDeletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts");

            migrationBuilder.AddForeignKey(
                name: "FK_ChainProducts_Categories_CategoryId",
                table: "ChainProducts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

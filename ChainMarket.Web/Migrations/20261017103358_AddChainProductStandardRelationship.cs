using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChainProductStandardRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChainStandardId",
                table: "ChainProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChainProducts_ChainStandardId",
                table: "ChainProducts",
                column: "ChainStandardId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChainProducts_ChainStandards_ChainStandardId",
                table: "ChainProducts",
                column: "ChainStandardId",
                principalTable: "ChainStandards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChainProducts_ChainStandards_ChainStandardId",
                table: "ChainProducts");

            migrationBuilder.DropIndex(
                name: "IX_ChainProducts_ChainStandardId",
                table: "ChainProducts");

            migrationBuilder.DropColumn(
                name: "ChainStandardId",
                table: "ChainProducts");
        }
    }
}

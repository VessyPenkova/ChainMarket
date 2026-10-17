using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChainProductNomenclatureRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChainNomenclatureId",
                table: "ChainProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChainProducts_ChainNomenclatureId",
                table: "ChainProducts",
                column: "ChainNomenclatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChainProducts_ChainNomenclatures_ChainNomenclatureId",
                table: "ChainProducts",
                column: "ChainNomenclatureId",
                principalTable: "ChainNomenclatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChainProducts_ChainNomenclatures_ChainNomenclatureId",
                table: "ChainProducts");

            migrationBuilder.DropIndex(
                name: "IX_ChainProducts_ChainNomenclatureId",
                table: "ChainProducts");

            migrationBuilder.DropColumn(
                name: "ChainNomenclatureId",
                table: "ChainProducts");
        }
    }
}

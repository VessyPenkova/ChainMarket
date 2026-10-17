using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChainNomenclature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChainNomenclatures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChainStandardId = table.Column<int>(type: "INTEGER", nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    PitchMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    InnerWidthMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    RollerDiameterMm = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainNomenclatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChainNomenclatures_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChainNomenclatures_ChainStandards_ChainStandardId",
                        column: x => x.ChainStandardId,
                        principalTable: "ChainStandards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainNomenclatures_CategoryId_Code",
                table: "ChainNomenclatures",
                columns: new[] { "CategoryId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChainNomenclatures_ChainStandardId",
                table: "ChainNomenclatures",
                column: "ChainStandardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChainNomenclatures");
        }
    }
}

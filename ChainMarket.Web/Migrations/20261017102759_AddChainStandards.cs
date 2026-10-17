using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddChainStandards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChainStandards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChainStandards", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChainStandards_Code",
                table: "ChainStandards",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChainStandards");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    public partial class AddProductSellingUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SellingUnit",
                table: "ChainProducts",
                type: "TEXT",
                maxLength: 10,
                nullable: false,
                defaultValue: "m");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingUnit",
                table: "ChainProducts");
        }
    }
}
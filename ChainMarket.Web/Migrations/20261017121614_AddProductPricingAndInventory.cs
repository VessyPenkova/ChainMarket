using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    public partial class AddProductPricingAndInventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ChainProducts",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "ManufacturerId",
                table: "ChainProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfStrands",
                table: "ChainProducts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePricePerUnit",
                table: "ChainProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPricePerUnit",
                table: "ChainProducts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StockQuantity",
                table: "ChainProducts",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            // Preserve existing purchase prices, selling prices and stock.
            migrationBuilder.Sql(@"
                UPDATE ChainProducts
                SET PurchasePricePerUnit = PurchasePricePerMeter,
                    SellingPricePerUnit = SellingPricePerMeter,
                    StockQuantity = StockQuantityMeters
                WHERE SellingUnit = 'm';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("IsActive", "ChainProducts");
            migrationBuilder.DropColumn("ManufacturerId", "ChainProducts");
            migrationBuilder.DropColumn("NumberOfStrands", "ChainProducts");
            migrationBuilder.DropColumn("PurchasePricePerUnit", "ChainProducts");
            migrationBuilder.DropColumn("SellingPricePerUnit", "ChainProducts");
            migrationBuilder.DropColumn("StockQuantity", "ChainProducts");
        }
    }
}
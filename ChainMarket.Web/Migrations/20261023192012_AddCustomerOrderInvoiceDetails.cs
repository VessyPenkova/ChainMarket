
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerOrderInvoiceDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BillingAddress",
                table: "CustomerOrders",
                type: "TEXT",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEik",
                table: "CustomerOrders",
                type: "TEXT",
                maxLength: 13,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "CustomerOrders",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyVatNumber",
                table: "CustomerOrders",
                type: "TEXT",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerType",
                table: "CustomerOrders",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "Individual");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BillingAddress",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CompanyEik",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CompanyVatNumber",
                table: "CustomerOrders");

            migrationBuilder.DropColumn(
                name: "CustomerType",
                table: "CustomerOrders");
        }
    }
}

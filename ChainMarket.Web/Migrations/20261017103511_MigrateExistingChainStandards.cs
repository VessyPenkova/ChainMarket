using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChainMarket.Web.Migrations
{
    public partial class MigrateExistingChainStandards : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO ChainStandards (Code)
                SELECT DISTINCT TRIM(Standard)
                FROM ChainProducts
                WHERE Standard IS NOT NULL
                  AND TRIM(Standard) <> ''
                  AND NOT EXISTS (
                      SELECT 1
                      FROM ChainStandards
                      WHERE Code = TRIM(Standard)
                  );
            ");

            migrationBuilder.Sql(@"
                UPDATE ChainProducts
                SET ChainStandardId = (
                    SELECT Id
                    FROM ChainStandards
                    WHERE Code = TRIM(ChainProducts.Standard)
                )
                WHERE Standard IS NOT NULL
                  AND TRIM(Standard) <> ''
                  AND ChainStandardId IS NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ChainProducts
                SET ChainStandardId = NULL
                WHERE Standard IS NOT NULL
                  AND TRIM(Standard) <> ''
                  AND ChainStandardId IN (
                      SELECT Id
                      FROM ChainStandards
                      WHERE Code = TRIM(ChainProducts.Standard)
                  );
            ");
        }
    }
}
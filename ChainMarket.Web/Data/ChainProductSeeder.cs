
using ChainMarket.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Data
{
    public static class ChainProductSeeder
    {
        // Purchase and selling prices: estimated EUR, excluding VAT.
        // m = metre, pcs = piece.
        // Codes for links ending CL are internal ChainMarket codes.
        private static readonly (
            int Category,
            string Code,
            decimal Purchase,
            decimal Selling,
            string Unit)[] Items =
        {
            // 1. РОЛКОВИ ВЕРИГИ
            (1, "06B-1", 3.50m, 5.90m, "m"),
            (1, "06B-2", 7.00m, 11.90m, "m"),
            (1, "06B-3", 10.50m, 17.90m, "m"),
            (1, "08B-1", 4.50m, 7.90m, "m"),
            (1, "08B-2", 9.00m, 15.90m, "m"),
            (1, "08B-3", 13.50m, 23.90m, "m"),
            (1, "10B-1", 6.50m, 10.90m, "m"),
            (1, "10B-2", 13.00m, 21.90m, "m"),
            (1, "10B-3", 19.50m, 32.90m, "m"),
            (1, "12B-1", 9.00m, 15.50m, "m"),

            // 2. ТРАНСПОРТЬОРНИ ВЕРИГИ
            (2, "M40P63", 23m, 39m, "m"),
            (2, "M40P80", 25m, 42m, "m"),
            (2, "M40P100", 27m, 46m, "m"),
            (2, "M40P125", 30m, 51m, "m"),
            (2, "M40P160", 33m, 56m, "m"),
            (2, "M56P80", 35m, 59m, "m"),
            (2, "M56P100", 38m, 65m, "m"),
            (2, "M56P125", 42m, 72m, "m"),
            (2, "M80P80", 48m, 82m, "m"),
            (2, "M80P100", 52m, 89m, "m"),

            // 3. ПОДЕМНИ ВЕРИГИ
            (3, "AL422", 12m, 21m, "m"),
            (3, "AL444", 21m, 36m, "m"),
            (3, "AL466", 30m, 51m, "m"),
            (3, "BL423", 16m, 27m, "m"),
            (3, "BL434", 22m, 38m, "m"),
            (3, "BL446", 32m, 55m, "m"),
            (3, "AL522", 17m, 29m, "m"),
            (3, "AL544", 29m, 49m, "m"),
            (3, "BL523", 22m, 38m, "m"),
            (3, "BL534", 31m, 53m, "m"),

            // 4. СЕЛСКОСТОПАНСКИ ВЕРИГИ
            (4, "CA550", 17m, 29m, "m"),
            (4, "CA550V", 20m, 34m, "m"),
            (4, "CA555", 17m, 29m, "m"),
            (4, "CA557", 23m, 39m, "m"),
            (4, "CA620", 21m, 36m, "m"),
            (4, "CA650", 30m, 51m, "m"),
            (4, "A550", 16m, 27m, "m"),
            (4, "A555", 16m, 27m, "m"),
            (4, "A557", 22m, 37m, "m"),
            (4, "A620", 20m, 34m, "m"),

            // 5. НЕРЪЖДАЕМИ И СПЕЦИАЛНИ ВЕРИГИ
            (5, "06BSS", 12m, 21m, "m"),
            (5, "08BSS", 18m, 31m, "m"),
            (5, "10BSS", 25m, 43m, "m"),
            (5, "12BSS", 34m, 58m, "m"),
            (5, "16BSS", 59m, 100m, "m"),
            (5, "06B-2SS", 24m, 41m, "m"),
            (5, "08B-2SS", 36m, 61m, "m"),
            (5, "10B-2SS", 50m, 85m, "m"),
            (5, "12B-2SS", 68m, 116m, "m"),
            (5, "16B-2SS", 118m, 201m, "m"),

            // 6. СВЪРЗВАЩИ ЗВЕНА - НА БРОЙ
            (6, "06B-1-CL", 1.80m, 3.10m, "pcs"),
            (6, "06B-2-CL", 3.80m, 6.50m, "pcs"),
            (6, "06B-3-CL", 5.90m, 10.00m, "pcs"),
            (6, "08B-1-CL", 1.70m, 2.90m, "pcs"),
            (6, "08B-2-CL", 3.50m, 5.95m, "pcs"),
            (6, "08B-3-CL", 5.50m, 9.35m, "pcs"),
            (6, "10B-1-CL", 1.90m, 3.25m, "pcs"),
            (6, "10B-2-CL", 4.00m, 6.80m, "pcs"),
            (6, "10B-3-CL", 6.20m, 10.55m, "pcs"),
            (6, "12B-1-CL", 2.20m, 3.75m, "pcs")
        };

        public static async Task<int> SeedAsync(
            ApplicationDbContext context)
        {
            int added = 0;

            var existingProducts = await context.ChainProducts
                .ToListAsync();

            var nomenclatures = await context.ChainNomenclatures
                .ToListAsync();

            foreach (var item in Items)
            {
                var nomenclature = nomenclatures.FirstOrDefault(n =>
                    n.CategoryId == item.Category &&
                    n.Code == item.Code);

                if (nomenclature == null)
                {
                    nomenclature = new ChainNomenclature
                    {
                        CategoryId = item.Category,
                        Code = item.Code
                    };

                    context.ChainNomenclatures.Add(nomenclature);
                    nomenclatures.Add(nomenclature);
                }

                var existing = existingProducts.FirstOrDefault(p =>
                    p.CategoryId == item.Category &&
                    p.ChainCode == item.Code);

                if (existing != null)
                {
                    // Never overwrite existing commercial data.
                    continue;
                }

                string name = item.Category switch
                {
                    1 => $"Ролкова верига {item.Code}",
                    2 => $"Транспортьорна верига {item.Code}",
                    3 => $"Подемна верига {item.Code}",
                    4 => $"Селскостопанска верига {item.Code}",
                    5 => $"Неръждаема верига {item.Code}",
                    6 => $"Свързващо звено {item.Code}",
                    _ => item.Code
                };

                int? strands = null;

                if (item.Category == 1 || item.Category == 6)
                {
                    var parts = item.Code.Split('-');

                    if (parts.Length > 1 &&
                        int.TryParse(parts[1], out int count))
                    {
                        strands = count;
                    }
                }

                var product = new ChainProduct
                {
                    Name = name,
                    CategoryId = item.Category,
                    ChainCode = item.Code,
                    ChainNomenclature = nomenclature,
                    NumberOfStrands = strands,
                    SellingUnit = item.Unit,
                    PurchasePricePerUnit = item.Purchase,
                    SellingPricePerUnit = item.Selling,
                    StockQuantity = 0m,
                    IsActive = true
                };

                context.ChainProducts.Add(product);
                existingProducts.Add(product);

                added++;
            }

            await context.SaveChangesAsync();

            return added;
        }
    }
}

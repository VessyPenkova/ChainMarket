using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Services
{
    public class ChainNomenclatureImportService
    {
        private readonly ApplicationDbContext _context;

        public ChainNomenclatureImportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> ImportAsync(
            IEnumerable<ChainNomenclature> items)
        {
            int added = 0;

            foreach (var item in items)
            {
                bool exists = await _context.ChainNomenclatures
                    .AnyAsync(n =>
                        n.CategoryId == item.CategoryId &&
                        n.Code == item.Code);

                if (exists)
                    continue;

                _context.ChainNomenclatures.Add(item);
                added++;
            }

            await _context.SaveChangesAsync();

            return added;
        }
    }
}
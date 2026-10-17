
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Controllers
{
    public class ChainProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChainProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            if (!categoryId.HasValue)
            {
                var categories = await _context.Categories
                    .AsNoTracking()
                    .OrderBy(c => c.Id)
                    .ToListAsync();

                return View("Categories", categories);
            }

            var category = await _context.Categories
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Id == categoryId.Value);

            if (category == null)
                return NotFound();

            ViewBag.CategoryName = category.Name;

            var products = await _context.ChainProducts
                .Include(p => p.Category)
                .Include(p => p.ChainStandard)
                .Include(p => p.ChainNomenclature)
                .Where(p => p.CategoryId == categoryId.Value && p.IsActive)
                .AsNoTracking()
                .OrderBy(p => p.ChainCode)
                .ToListAsync();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,CategoryId,ChainStandardId,ChainNomenclatureId,ChainCode")]
            ChainProduct product)
        {
            if (product.ChainNomenclatureId.HasValue)
            {
                bool valid = await _context.ChainNomenclatures
                    .AnyAsync(n =>
                        n.Id == product.ChainNomenclatureId.Value &&
                        n.CategoryId == product.CategoryId);

                if (!valid)
                {
                    ModelState.AddModelError(
                        nameof(product.ChainNomenclatureId),
                        "Номенклатурата не принадлежи към категорията.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns();
                return View(product);
            }

            _context.ChainProducts.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index),
                new { categoryId = product.CategoryId });
        }

        private async Task LoadDropdowns()
        {
            ViewBag.Categories = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToListAsync();

            ViewBag.Standards = await _context.ChainStandards
                .AsNoTracking()
                .OrderBy(s => s.Code)
                .ToListAsync();

            ViewBag.Nomenclatures = await _context.ChainNomenclatures
                .AsNoTracking()
                .OrderBy(n => n.Code)
                .ToListAsync();
        }
    }
}

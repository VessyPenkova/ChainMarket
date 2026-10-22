
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService _cartService;
        private readonly ShoppingCartSessionStore _sessionStore;

        public ShoppingCartController(ApplicationDbContext context, ShoppingCartService cartService, ShoppingCartSessionStore sessionStore)
        {
            _context = context;
            _cartService = cartService;
            _sessionStore = sessionStore;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_sessionStore.GetItems());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, decimal quantity = 1m)
        {
            var product = await _context.ChainProducts
                .AsNoTracking()
                .SingleOrDefaultAsync(p => p.Id == productId && p.IsActive);

            if (product == null)
                return NotFound();

            if (quantity <= 0 || (product.SellingUnit == "pcs" && quantity != decimal.Truncate(quantity)))
                return BadRequest();

            var cart = _sessionStore.GetItems();

            var item = new ShoppingCartItem
            {
                ChainProductId = product.Id,
                ProductName = product.Name,
                ChainCode = product.ChainCode,
                SellingUnit = product.SellingUnit,
                Quantity = quantity,
                UnitPrice = product.SellingPricePerUnit ?? product.SellingPricePerMeter
            };

            try
            {
                _cartService.AddItem(cart, item);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }

            _sessionStore.SaveItems(cart);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int productId, decimal quantity)
        {
            var cart = _sessionStore.GetItems();

            try
            {
                _cartService.UpdateQuantity(cart, productId, quantity);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }

            _sessionStore.SaveItems(cart);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var cart = _sessionStore.GetItems();

            _cartService.RemoveItem(cart, productId);
            _sessionStore.SaveItems(cart);

            return RedirectToAction(nameof(Index));
        }
    }
}

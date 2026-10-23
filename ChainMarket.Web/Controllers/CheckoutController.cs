
using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainMarket.Web.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CheckoutOrderService _checkoutOrderService;
        private readonly ShoppingCartSessionStore _sessionStore;

        public CheckoutController(CheckoutOrderService checkoutOrderService, ShoppingCartSessionStore sessionStore)
        {
            _checkoutOrderService = checkoutOrderService;
            _sessionStore = sessionStore;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = _sessionStore.GetItems();

            if (cart.Count == 0)
                return RedirectToAction("Index", "ShoppingCart");

            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var cart = _sessionStore.GetItems();

            if (cart.Count == 0)
                return RedirectToAction("Index", "ShoppingCart");

            if (!ModelState.IsValid)
                return View(model);

            try
            {
                var order = await _checkoutOrderService.CreateOrderAsync(model, cart);

                _sessionStore.SaveItems(new List<ShoppingCartItem>());

                TempData["ConfirmedOrderId"] = order.Id;

                return RedirectToAction(nameof(Success));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Success()
        {
            if (TempData["ConfirmedOrderId"] is not int orderId)
                return RedirectToAction("Index", "ShoppingCart");

            ViewBag.OrderId = orderId;

            return View();
        }
    }
}

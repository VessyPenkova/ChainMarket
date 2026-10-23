
using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Web.Services
{
    public class CheckoutOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService _cartService;

        public CheckoutOrderService(ApplicationDbContext context, ShoppingCartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<CustomerOrder> CreateOrderAsync(CheckoutViewModel checkout, List<ShoppingCartItem> cart)
        {
            if (checkout == null)
                throw new ArgumentNullException(nameof(checkout));

            if (cart == null || cart.Count == 0)
                throw new InvalidOperationException("The shopping cart is empty.");

            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(checkout, new ValidationContext(checkout), validationResults, true))
                throw new ValidationException(validationResults[0].ErrorMessage);

            if (cart.Any(i => i.ChainProductId <= 0))
                throw new InvalidOperationException("The shopping cart contains an invalid product.");

            if (cart.Select(i => i.ChainProductId).Distinct().Count() != cart.Count)
                throw new InvalidOperationException("The shopping cart contains duplicate products.");

            var productIds = cart.Select(i => i.ChainProductId).ToList();

            var products = await _context.ChainProducts
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id) && p.IsActive)
                .ToDictionaryAsync(p => p.Id);

            var items = new List<CustomerOrderItem>();

            foreach (var cartItem in cart)
            {
                if (!products.TryGetValue(cartItem.ChainProductId, out var product))
                    throw new InvalidOperationException("A product is no longer available.");

                if (cartItem.Quantity <= 0)
                    throw new InvalidOperationException("Invalid product quantity.");

                if (product.SellingUnit != "m" && product.SellingUnit != "pcs")
                    throw new InvalidOperationException("Invalid product selling unit.");

                if (product.SellingUnit == "pcs" && cartItem.Quantity != decimal.Truncate(cartItem.Quantity))
                    throw new InvalidOperationException("Piece quantities must be whole numbers.");

                decimal price = product.SellingPricePerUnit ?? product.SellingPricePerMeter;

                if (price < 0)
                    throw new InvalidOperationException("Invalid product price.");

                items.Add(new CustomerOrderItem
                {
                    ChainProductId = product.Id,
                    ProductName = product.Name,
                    SellingUnit = product.SellingUnit,
                    Quantity = cartItem.Quantity,
                    UnitPrice = price
                });
            }

            var orderItemsTotal = items.Select(i => new ShoppingCartItem
            {
                ChainProductId = i.ChainProductId,
                ProductName = i.ProductName,
                SellingUnit = i.SellingUnit,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList();

            decimal totalWithoutVat = Math.Round(_cartService.CalculateTotal(orderItemsTotal), 2, MidpointRounding.AwayFromZero);
            decimal vatAmount = Math.Round(totalWithoutVat * 0.20m, 2, MidpointRounding.AwayFromZero);

            var order = new CustomerOrder
            {
                CustomerName = checkout.CustomerName,
                Email = checkout.Email,
                Phone = checkout.Phone,
                DeliveryAddress = checkout.DeliveryAddress,
                OrderDate = DateTime.UtcNow,
                TotalWithoutVat = totalWithoutVat,
                VatAmount = vatAmount,
                TotalWithVat = totalWithoutVat + vatAmount
            };

            _context.CustomerOrders.Add(order);

            foreach (var item in items)
            {
                item.CustomerOrder = order;
                _context.CustomerOrderItems.Add(item);
            }

            await _context.SaveChangesAsync();

            return order;
        }
    }
}

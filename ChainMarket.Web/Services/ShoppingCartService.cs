
using ChainMarket.Web.Models;

namespace ChainMarket.Web.Services
{
    public class ShoppingCartService
    {
        public void AddItem(List<ShoppingCartItem> cart, ShoppingCartItem item)
        {
            ValidateQuantity(item.Quantity, item.SellingUnit);

            if (item.UnitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(item.UnitPrice));

            var existing = cart.FirstOrDefault(x => x.ChainProductId == item.ChainProductId);

            if (existing != null)
            {
                ValidateQuantity(existing.Quantity + item.Quantity, existing.SellingUnit);
                existing.Quantity += item.Quantity;
                return;
            }

            cart.Add(item);
        }

        public void UpdateQuantity(List<ShoppingCartItem> cart, int productId, decimal quantity)
        {
            var item = cart.FirstOrDefault(x => x.ChainProductId == productId);

            if (item == null)
                throw new InvalidOperationException("Product not found in cart.");

            ValidateQuantity(quantity, item.SellingUnit);

            item.Quantity = quantity;
        }

        public void RemoveItem(List<ShoppingCartItem> cart, int productId)
        {
            cart.RemoveAll(x => x.ChainProductId == productId);
        }

        public decimal CalculateTotal(List<ShoppingCartItem> cart)
        {
            return cart.Sum(x => x.TotalPrice);
        }

        private void ValidateQuantity(decimal quantity, string sellingUnit)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            if (sellingUnit != "m" && sellingUnit != "pcs")
                throw new ArgumentException("Invalid selling unit.", nameof(sellingUnit));

            if (sellingUnit == "pcs" && quantity != decimal.Truncate(quantity))
                throw new ArgumentException("Piece quantities must be whole numbers.", nameof(quantity));
        }
    }
}

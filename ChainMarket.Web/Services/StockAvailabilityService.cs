
using ChainMarket.Web.Models;

namespace ChainMarket.Web.Services
{
    public class StockAvailabilityService
    {
        public bool IsInStock(ChainProduct product, ChainOrder order)
        {
            return product.StockQuantityMeters >= order.TotalMeters;
        }

        public decimal GetMissingQuantityMeters(
            ChainProduct product, ChainOrder order)
        {
            return Math.Max(
                0m,
                order.TotalMeters - product.StockQuantityMeters);
        }

        public bool HasAvailableStock(
            ChainProduct product, decimal quantity)
        {
            if (quantity <= 0)
                return false;

            return product.StockQuantity >= quantity;
        }

        public decimal GetMissingQuantity(
            ChainProduct product, decimal quantity)
        {
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            return Math.Max(0m, quantity - product.StockQuantity);
        }
    }
}

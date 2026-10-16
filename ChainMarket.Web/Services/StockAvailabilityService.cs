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
    }
}
using ChainMarket.Web.Models;

namespace ChainMarket.Web.Services
{
    public class StockReceiptService
    {
        public decimal CalculateUpdatedStock(
            ChainProduct product,
            StockReceipt receipt)
        {
            if (product.Id != receipt.ChainProductId)
            {
                throw new InvalidOperationException(
                    "The receipt does not match the product.");
            }

            if (receipt.QuantityMeters <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(receipt.QuantityMeters));
            }

            if (product.StockQuantityMeters < 0)
            {
                throw new InvalidOperationException(
                    "Current stock cannot be negative.");
            }

            return product.StockQuantityMeters
                   + receipt.QuantityMeters;
        }
    }
}
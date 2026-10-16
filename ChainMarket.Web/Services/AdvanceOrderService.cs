using ChainMarket.Web.Models;

namespace ChainMarket.Web.Services
{
    public class AdvanceOrderService
    {
        private readonly StockAvailabilityService stockService = new();

        public bool RequiresAdvanceOrder(
            ChainProduct product,
            ChainOrder order)
        {
            return !stockService.IsInStock(product, order);
        }

        public PurchaseOrder CreatePurchaseOrderDraft(
            ChainProduct product,
            ChainOrder order,
            int supplierId)
        {
            if (supplierId <= 0)
                throw new ArgumentOutOfRangeException(nameof(supplierId));

            decimal missingMeters =
                stockService.GetMissingQuantityMeters(product, order);

            if (missingMeters <= 0)
                throw new InvalidOperationException(
                    "No additional stock is required.");

            return new PurchaseOrder
            {
                SupplierId = supplierId,
                ChainProductId = product.Id,
                QuantityMeters = missingMeters,
                PurchasePricePerMeter = product.PurchasePricePerMeter,
                Status = "Draft",
                Notes = "Prepared for customer advance order."
            };
        }
    }
}
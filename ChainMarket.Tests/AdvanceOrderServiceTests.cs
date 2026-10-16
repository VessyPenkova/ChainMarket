using ChainMarket.Web.Models;
using ChainMarket.Web.Services;

namespace ChainMarket.Tests
{
    public class AdvanceOrderServiceTests
    {
        private readonly AdvanceOrderService service = new();

        [Test]
        public void RequiresAdvanceOrder_WhenStockIsInsufficient_ShouldReturnTrue()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 30m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 5
            };

            Assert.That(service.RequiresAdvanceOrder(product, order), Is.True);
        }

        [Test]
        public void RequiresAdvanceOrder_WhenStockIsSufficient_ShouldReturnFalse()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 50m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 3
            };

            Assert.That(service.RequiresAdvanceOrder(product, order), Is.False);
        }

        [Test]
        public void CreatePurchaseOrderDraft_ShouldUseOnlyMissingQuantity()
        {
            var product = new ChainProduct
            {
                Id = 1,
                StockQuantityMeters = 30m,
                PurchasePricePerMeter = 8m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 5
            };

            var draft = service.CreatePurchaseOrderDraft(product, order, 2);

            Assert.Multiple(() =>
            {
                Assert.That(draft.SupplierId, Is.EqualTo(2));
                Assert.That(draft.ChainProductId, Is.EqualTo(1));
                Assert.That(draft.QuantityMeters, Is.EqualTo(20m));
                Assert.That(draft.PurchasePricePerMeter, Is.EqualTo(8m));
                Assert.That(draft.TotalWithoutVat, Is.EqualTo(160m));
                Assert.That(draft.Status, Is.EqualTo("Draft"));
            });
        }

        [Test]
        public void CreatePurchaseOrderDraft_WhenStockIsSufficient_ShouldThrow()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 50m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 3
            };

            Assert.Throws<InvalidOperationException>(() =>
                service.CreatePurchaseOrderDraft(product, order, 1));
        }

        [Test]
        public void CreatePurchaseOrderDraft_WithInvalidSupplier_ShouldThrow()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 10m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 3
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                service.CreatePurchaseOrderDraft(product, order, 0));
        }
    }
}
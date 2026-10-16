using ChainMarket.Web.Models;
using ChainMarket.Web.Services;

namespace ChainMarket.Tests
{
    public class StockReceiptServiceTests
    {
        private readonly StockReceiptService service = new();

        [Test]
        public void CalculateUpdatedStock_ShouldIncreaseStock()
        {
            var product = new ChainProduct
            {
                Id = 1,
                StockQuantityMeters = 30m
            };

            var receipt = new StockReceipt
            {
                ChainProductId = 1,
                QuantityMeters = 50m
            };

            Assert.That(
                service.CalculateUpdatedStock(product, receipt),
                Is.EqualTo(80m));
        }

        [Test]
        public void CalculateUpdatedStock_WithWrongProduct_ShouldThrow()
        {
            var product = new ChainProduct { Id = 1 };

            var receipt = new StockReceipt
            {
                ChainProductId = 2,
                QuantityMeters = 50m
            };

            Assert.Throws<InvalidOperationException>(() =>
                service.CalculateUpdatedStock(product, receipt));
        }

        [Test]
        public void CalculateUpdatedStock_WithZeroQuantity_ShouldThrow()
        {
            var product = new ChainProduct { Id = 1 };

            var receipt = new StockReceipt
            {
                ChainProductId = 1,
                QuantityMeters = 0m
            };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
                service.CalculateUpdatedStock(product, receipt));
        }

        [Test]
        public void CalculateUpdatedStock_WithNegativeStock_ShouldThrow()
        {
            var product = new ChainProduct
            {
                Id = 1,
                StockQuantityMeters = -5m
            };

            var receipt = new StockReceipt
            {
                ChainProductId = 1,
                QuantityMeters = 10m
            };

            Assert.Throws<InvalidOperationException>(() =>
                service.CalculateUpdatedStock(product, receipt));
        }
    }
}
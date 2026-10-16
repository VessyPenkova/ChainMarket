using ChainMarket.Web.Models;
using ChainMarket.Web.Services;

namespace ChainMarket.Tests
{
    public class StockAvailabilityServiceTests
    {
        private StockAvailabilityService service = null!;

        [SetUp]
        public void Setup()
        {
            service = new StockAvailabilityService();
        }

        [Test]
        public void IsInStock_WhenQuantityIsSufficient_ShouldReturnTrue()
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

            Assert.That(service.IsInStock(product, order), Is.True);
        }

        [Test]
        public void IsInStock_WhenQuantityIsInsufficient_ShouldReturnFalse()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 20m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 3
            };

            Assert.That(service.IsInStock(product, order), Is.False);
        }

        [Test]
        public void GetMissingQuantityMeters_ShouldReturnCorrectShortage()
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

            Assert.That(
                service.GetMissingQuantityMeters(product, order),
                Is.EqualTo(20m));
        }

        [Test]
        public void GetMissingQuantityMeters_WhenStockIsSufficient_ShouldReturnZero()
        {
            var product = new ChainProduct
            {
                StockQuantityMeters = 50m
            };

            var order = new ChainOrder
            {
                PackageSizeMeters = "5",
                Quantity = 2
            };

            Assert.That(
                service.GetMissingQuantityMeters(product, order),
                Is.EqualTo(0m));
        }
    }
}
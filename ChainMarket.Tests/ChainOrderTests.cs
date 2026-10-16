using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class ChainOrderTests
    {
        [TestCase("1", 3, 3)]
        [TestCase("5", 2, 10)]
        [TestCase("10", 3, 30)]
        [TestCase("50", 2, 100)]
        public void ChainOrder_ShouldCalculateTotalMeters(
            string packageSize, int quantity, int expectedMeters)
        {
            var order = new ChainOrder
            {
                PackageSizeMeters = packageSize,
                Quantity = quantity
            };

            Assert.That(order.TotalMeters, Is.EqualTo(expectedMeters));
        }

        [Test]
        public void ChainOrder_ShouldCalculateTotalWithVat()
        {
            var order = new ChainOrder
            {
                PackageSizeMeters = "10",
                Quantity = 2,
                SellingPricePerMeter = 15m
            };

            Assert.That(order.TotalWithoutVat, Is.EqualTo(300m));
            Assert.That(order.VatAmount, Is.EqualTo(60m));
            Assert.That(order.TotalWithVat, Is.EqualTo(360m));
        }

        [Test]
        public void ChainOrder_WithInvalidPackageSize_ShouldFailValidation()
        {
            var order = new ChainOrder
            {
                CustomerName = "Test Customer",
                PackageSizeMeters = "7"
            };

            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                order,
                new ValidationContext(order),
                results,
                true);

            Assert.That(isValid, Is.False);
        }
    }
}
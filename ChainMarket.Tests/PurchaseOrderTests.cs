using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class PurchaseOrderTests
    {
        [Test]
        public void PurchaseOrder_ShouldCalculateTotalWithoutVat()
        {
            // Arrange
            var order = new PurchaseOrder
            {
                QuantityMeters = 50m,
                PurchasePricePerMeter = 12m
            };

            // Assert
            Assert.That(order.TotalWithoutVat, Is.EqualTo(600m));
        }

        [Test]
        public void PurchaseOrder_WithZeroQuantity_ShouldFailValidation()
        {
            // Arrange
            var order = new PurchaseOrder
            {
                SupplierId = 1,
                ChainProductId = 1,
                QuantityMeters = 0m,
                PurchasePricePerMeter = 12m
            };

            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                order,
                new ValidationContext(order),
                results,
                true);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
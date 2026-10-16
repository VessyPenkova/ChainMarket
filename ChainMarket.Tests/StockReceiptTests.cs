using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class StockReceiptTests
    {
        [Test]
        public void StockReceipt_ShouldCalculateInvoiceTotals()
        {
            // Arrange
            var receipt = new StockReceipt
            {
                QuantityMeters = 50m,
                PurchasePricePerMeter = 10m
            };

            // Assert
            Assert.That(receipt.TotalWithoutVat, Is.EqualTo(500m));
            Assert.That(receipt.VatAmount, Is.EqualTo(100m));
            Assert.That(receipt.TotalWithVat, Is.EqualTo(600m));
        }

        [Test]
        public void StockReceipt_WithZeroQuantity_ShouldFailValidation()
        {
            // Arrange
            var receipt = new StockReceipt
            {
                InvoiceNumber = "INV-001",
                QuantityMeters = 0m,
                PurchasePricePerMeter = 10m
            };

            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                receipt,
                new ValidationContext(receipt),
                results,
                true);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
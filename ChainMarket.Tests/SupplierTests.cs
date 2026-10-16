using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class SupplierTests
    {
        [Test]
        public void Supplier_WithoutName_ShouldFailValidation()
        {
            // Arrange
            var supplier = new Supplier { Name = "" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                supplier,
                new ValidationContext(supplier),
                results,
                true);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
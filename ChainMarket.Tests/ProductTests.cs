using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class ProductTests
    {
        [Test]
        public void Product_WithoutName_ShouldFailValidation()
        {
            // Arrange
            var product = new Product { Name = "" };
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                product,
                new ValidationContext(product),
                results,
                true);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
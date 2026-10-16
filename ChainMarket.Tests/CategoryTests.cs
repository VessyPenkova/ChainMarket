using System.ComponentModel.DataAnnotations;
using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class CategoryTests
    {
        [Test]
        public void Category_WithoutName_ShouldFailValidation()
        {
            // Arrange
            var category = new Category
            {
                Name = ""
            };

            var context = new ValidationContext(category);
            var results = new List<ValidationResult>();

            // Act
            bool isValid = Validator.TryValidateObject(
                category, context, results, true);

            // Assert
            Assert.That(isValid, Is.False);
        }
    }
}
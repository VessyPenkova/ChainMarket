using ChainMarket.Web.Models;

namespace ChainMarket.Tests
{
    public class ChainCategoriesTests
    {
        [Test]
        public void ChainCategories_ShouldContainSixCategories()
        {
            // Act
            var categories = ChainCategories.Names;

            // Assert
            Assert.That(categories, Has.Length.EqualTo(6));
            Assert.That(categories, Does.Contain("Ролкови вериги"));
            Assert.That(categories, Does.Contain("Транспортьорни вериги"));
        }
    }
}
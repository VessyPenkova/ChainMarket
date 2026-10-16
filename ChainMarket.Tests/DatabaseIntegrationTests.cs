using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Tests
{
    public class DatabaseIntegrationTests
    {
        [Test]
        public async Task Category_ShouldBeSavedAndRetrievedFromDatabase()
        {
            // Arrange - isolated in-memory SQLite database
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.Categories.Add(new Category
                {
                    Name = "Ролкови вериги",
                    Description = "Industrial roller chains"
                });

                await context.SaveChangesAsync();
            }

            // Act - retrieve using a new DbContext
            using var verificationContext = new ApplicationDbContext(options);

            var category = await verificationContext.Categories
                .SingleAsync();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(category.Name, Is.EqualTo("Ролкови вериги"));
                Assert.That(category.Description,
                    Is.EqualTo("Industrial roller chains"));
            });
        }
    }
}
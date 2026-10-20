using ChainMarket.Web.Controllers;
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Tests
{
    public class ChainProductsControllerTests
    {
        [Test]
        public async Task Index_ShouldReturnProductsWithCategories()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            context.ChainProducts.Add(new ChainProduct
            {
                Name = "Ролкова верига 08B-1",
                Category = new Category { Name = "Ролкови вериги" }
            });

            await context.SaveChangesAsync();

            var controller = new ChainProductsController(context);

            var result = await controller.Index(context.ChainProducts.Single().CategoryId);

            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);

            var products = viewResult!.Model as List<ChainProduct>;

            Assert.Multiple(() =>
            {
                Assert.That(products, Is.Not.Null);
                Assert.That(products, Has.Count.EqualTo(1));
                Assert.That(products![0].Category?.Name,
                    Is.EqualTo("Ролкови вериги"));
            });
        }
    }
}
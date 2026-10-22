
using ChainMarket.Web.Controllers;
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Tests
{
    public class ShoppingCartControllerTests
    {
        [Test]
        public async Task Add_WithNonexistentProduct_ShouldReturnNotFound()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var controller = CreateController(context);

            var result = await controller.Add(99999, 1m);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task Add_WithInactiveProduct_ShouldReturnNotFound()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var product = new ChainProduct
            {
                Name = "Inactive Test Chain",
                CategoryId = 1,
                SellingUnit = "m",
                IsActive = false
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.Add(product.Id, 1m);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task Add_WithInvalidQuantity_ShouldReturnBadRequest(int quantity)
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var product = new ChainProduct
            {
                Name = "Test Roller Chain",
                CategoryId = 1,
                SellingUnit = "m",
                IsActive = true
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.Add(product.Id, quantity);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }

        [Test]
        public async Task Add_PiecesWithFractionalQuantity_ShouldReturnBadRequest()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var product = new ChainProduct
            {
                Name = "Test Connecting Link",
                CategoryId = 6,
                SellingUnit = "pcs",
                IsActive = true
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var controller = CreateController(context);

            var result = await controller.Add(product.Id, 1.5m);

            Assert.That(result, Is.InstanceOf<BadRequestResult>());
        }


        [Test]
        public void ShoppingCartSessionStore_ShouldPreserveItems()
        {
            var context = new DefaultHttpContext();
            var session = new TestSession();
            context.Session = session;

            var accessor = new HttpContextAccessor { HttpContext = context };
            var store = new ShoppingCartSessionStore(accessor);

            var items = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    ProductName = "Ролкова верига 06B-1",
                    SellingUnit = "m",
                    Quantity = 5m,
                    UnitPrice = 5.90m
                }
            };

            store.SaveItems(items);

            var restored = store.GetItems();

            Assert.Multiple(() =>
            {
                Assert.That(restored, Has.Count.EqualTo(1));
                Assert.That(restored[0].ChainProductId, Is.EqualTo(1));
                Assert.That(restored[0].Quantity, Is.EqualTo(5m));
                Assert.That(restored[0].TotalPrice, Is.EqualTo(29.50m));
            });
        }


        [Test]
        public async Task Add_WithValidProduct_ShouldSaveCorrectProductAndPriceToSession()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var product = new ChainProduct
            {
                Name = "Ролкова верига 06B-1",
                ChainCode = "06B-1",
                CategoryId = 1,
                SellingUnit = "m",
                SellingPricePerUnit = 5.90m,
                IsActive = true
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            var accessor = new HttpContextAccessor { HttpContext = httpContext };
            var sessionStore = new ShoppingCartSessionStore(accessor);
            var controller = new ShoppingCartController(context, new ShoppingCartService(), sessionStore);

            var result = await controller.Add(product.Id, 2m);
            var cart = sessionStore.GetItems();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
                Assert.That(((RedirectToActionResult)result).ActionName, Is.EqualTo("Index"));
                Assert.That(cart, Has.Count.EqualTo(1));
                Assert.That(cart[0].ChainProductId, Is.EqualTo(product.Id));
                Assert.That(cart[0].ProductName, Is.EqualTo(product.Name));
                Assert.That(cart[0].ChainCode, Is.EqualTo("06B-1"));
                Assert.That(cart[0].Quantity, Is.EqualTo(2m));
                Assert.That(cart[0].UnitPrice, Is.EqualTo(5.90m));
                Assert.That(cart[0].TotalPrice, Is.EqualTo(11.80m));
            });
        }


        [TestCase(0)]
        [TestCase(-2)]
        public async Task Update_WithInvalidQuantity_ShouldPreserveExistingCart(int quantity)
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            var accessor = new HttpContextAccessor { HttpContext = httpContext };
            var sessionStore = new ShoppingCartSessionStore(accessor);
            var controller = new ShoppingCartController(context, new ShoppingCartService(), sessionStore);

            sessionStore.SaveItems(new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    ProductName = "Ролкова верига 06B-1",
                    SellingUnit = "m",
                    Quantity = 5m,
                    UnitPrice = 5.90m
                }
            });
            
            var result = controller.Update(1, quantity);
            var cart = sessionStore.GetItems();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<BadRequestResult>());
                Assert.That(cart, Has.Count.EqualTo(1));
                Assert.That(cart[0].Quantity, Is.EqualTo(5m));
                Assert.That(cart[0].TotalPrice, Is.EqualTo(29.50m));
            });
        }

        private static ShoppingCartController CreateController(ApplicationDbContext context)
        {
            var cartService = new ShoppingCartService();
            var sessionStore = new ShoppingCartSessionStore(new HttpContextAccessor());

            return new ShoppingCartController(context, cartService, sessionStore);
        }
    }
}

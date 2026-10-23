
using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ChainMarket.Tests
{
    public class CheckoutOrderServiceTests
    {
        [Test]
        public async Task CreateOrder_WithTwoProducts_ShouldSaveOrderItemsAndVat()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var firstProduct = new ChainProduct
            {
                Name = "Ролкова верига 06B-1",
                CategoryId = 1,
                SellingUnit = "m",
                SellingPricePerUnit = 5.90m,
                IsActive = true
            };

            var secondProduct = new ChainProduct
            {
                Name = "Свързващо звено 06B-1",
                CategoryId = 6,
                SellingUnit = "pcs",
                SellingPricePerUnit = 3.10m,
                IsActive = true
            };

            context.ChainProducts.AddRange(firstProduct, secondProduct);
            await context.SaveChangesAsync();

            var checkout = new CheckoutViewModel
            {
                CustomerName = "Test Customer",
                Email = "customer@example.com",
                Phone = "+359888123456",
                DeliveryAddress = "Plovdiv, Bulgaria"
            };

            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = firstProduct.Id,
                    Quantity = 5m,
                    SellingUnit = "m",
                    UnitPrice = 1m
                },
                new ShoppingCartItem
                {
                    ChainProductId = secondProduct.Id,
                    Quantity = 2m,
                    SellingUnit = "pcs",
                    UnitPrice = 1m
                }
            };

            var service = new CheckoutOrderService(context, new ShoppingCartService());

            var order = await service.CreateOrderAsync(checkout, cart);

            var savedOrder = await context.CustomerOrders.SingleAsync();
            var savedItems = await context.CustomerOrderItems.OrderBy(i => i.ChainProductId).ToListAsync();

            Assert.Multiple(() =>
            {
                Assert.That(order.Id, Is.GreaterThan(0));
                Assert.That(savedOrder.CustomerName, Is.EqualTo("Test Customer"));
                Assert.That(savedItems, Has.Count.EqualTo(2));
                Assert.That(savedOrder.TotalWithoutVat, Is.EqualTo(35.70m));
                Assert.That(savedOrder.VatAmount, Is.EqualTo(7.14m));
                Assert.That(savedOrder.TotalWithVat, Is.EqualTo(42.84m));
                Assert.That(savedItems.Single(i => i.ChainProductId == firstProduct.Id).UnitPrice, Is.EqualTo(5.90m));
                Assert.That(savedItems.Single(i => i.ChainProductId == secondProduct.Id).UnitPrice, Is.EqualTo(3.10m));
            });
        }


        [Test]
        public async Task CreateOrder_Company_ShouldSaveInvoiceDetails()
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
                CategoryId = 1,
                SellingUnit = "m",
                SellingPricePerUnit = 5.90m,
                IsActive = true
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var checkout = new CheckoutViewModel
            {
                CustomerType = "Company",
                CustomerName = "Иван Петров",
                Email = "test@example.com",
                Phone = "+359888123456",
                DeliveryAddress = "гр. Пловдив, ул. Тестова 10",
                CompanyName = "Тест Компания ООД",
                CompanyEik = "123456789",
                CompanyVatNumber = "BG123456789",
                BillingAddress = "гр. Пловдив, ул. Примерна 20"
            };

            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = product.Id,
                    SellingUnit = "m",
                    Quantity = 2m,
                    UnitPrice = 5.90m
                }
            };

            var service = new CheckoutOrderService(context, new ShoppingCartService());

            await service.CreateOrderAsync(checkout, cart);

            var savedOrder = await context.CustomerOrders.SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(savedOrder.CustomerType, Is.EqualTo("Company"));
                Assert.That(savedOrder.CompanyName, Is.EqualTo("Тест Компания ООД"));
                Assert.That(savedOrder.CompanyEik, Is.EqualTo("123456789"));
                Assert.That(savedOrder.CompanyVatNumber, Is.EqualTo("BG123456789"));
                Assert.That(savedOrder.BillingAddress, Is.EqualTo("гр. Пловдив, ул. Примерна 20"));
                Assert.That(savedOrder.DeliveryAddress, Is.EqualTo("гр. Пловдив, ул. Тестова 10"));
            });
        }


        [Test]
        public async Task CreateOrder_Individual_ShouldNotSaveCompanyDetails()
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
                CategoryId = 1,
                SellingUnit = "m",
                SellingPricePerUnit = 5.90m,
                IsActive = true
            };

            context.ChainProducts.Add(product);
            await context.SaveChangesAsync();

            var checkout = new CheckoutViewModel
            {
                CustomerType = "Individual",
                CustomerName = "Иван Петров",
                Email = "test@example.com",
                Phone = "+359888123456",
                DeliveryAddress = "гр. Пловдив, ул. Тестова 10",
                CompanyName = "Тест Компания ООД",
                CompanyEik = "123456789",
                CompanyVatNumber = "BG123456789",
                BillingAddress = "гр. Пловдив, ул. Примерна 20"
            };

            var cart = new List<ShoppingCartItem>
             {
                 new ShoppingCartItem
                 {
                     ChainProductId = product.Id,
                     SellingUnit = "m",
                     Quantity = 2m,
                     UnitPrice = 5.90m
                 }
             };

            var service = new CheckoutOrderService(context, new ShoppingCartService());

            await service.CreateOrderAsync(checkout, cart);

            var savedOrder = await context.CustomerOrders.SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(savedOrder.CustomerType, Is.EqualTo("Individual"));
                Assert.That(savedOrder.CustomerName, Is.EqualTo("Иван Петров"));
                Assert.That(savedOrder.DeliveryAddress, Is.EqualTo("гр. Пловдив, ул. Тестова 10"));
                Assert.That(savedOrder.CompanyName, Is.Null);
                Assert.That(savedOrder.CompanyEik, Is.Null);
                Assert.That(savedOrder.CompanyVatNumber, Is.Null);
                Assert.That(savedOrder.BillingAddress, Is.Null);
            });
        }

        [Test]
        public void CreateOrder_WithEmptyCart_ShouldThrow()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            var service = new CheckoutOrderService(context, new ShoppingCartService());

            Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await service.CreateOrderAsync(new CheckoutViewModel(), new List<ShoppingCartItem>()));
        }
    }
}

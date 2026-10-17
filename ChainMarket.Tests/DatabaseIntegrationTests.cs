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

        [Test]
        public async Task ChainProduct_ShouldBelongToCategory()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var category = new Category { Name = "Ролкови вериги" };
                var product = new ChainProduct
                {
                    Name = "Ролкова верига 08B-1",
                    Category = category
                };

                context.ChainProducts.Add(product);
                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var savedProduct = await verificationContext.ChainProducts
                .Include(p => p.Category)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(savedProduct.Category, Is.Not.Null);
                Assert.That(savedProduct.Category!.Name,
                    Is.EqualTo("Ролкови вериги"));
                Assert.That(savedProduct.CategoryId, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task StockReceipt_ShouldBelongToSupplier()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var supplier = new Supplier { Name = "Test Supplier" };

                var product = new ChainProduct
                {
                    Name = "Test Chain",
                    Category = new Category { Name = "Ролкови вериги" }
                };

                context.StockReceipts.Add(new StockReceipt
                {
                    Supplier = supplier,
                    ChainProduct = product,
                    InvoiceNumber = "INV-001",
                    InvoiceDate = DateTime.Today,
                    QuantityMeters = 50m,
                    PurchasePricePerMeter = 10m
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var receipt = await verificationContext.StockReceipts
                .Include(r => r.Supplier)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(receipt.Supplier, Is.Not.Null);
                Assert.That(receipt.Supplier!.Name, Is.EqualTo("Test Supplier"));
                Assert.That(receipt.SupplierId, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task PurchaseOrder_ShouldBelongToSupplier()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.PurchaseOrders.Add(new PurchaseOrder
                {
                    Supplier = new Supplier { Name = "Test Manufacturer" },
                    ChainProduct = new ChainProduct
                    {
                        Name = "Test Chain",
                        Category = new Category { Name = "Ролкови вериги" }
                    },
                    QuantityMeters = 50m,
                    PurchasePricePerMeter = 10m
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var order = await verificationContext.PurchaseOrders
                .Include(p => p.Supplier)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(order.Supplier, Is.Not.Null);
                Assert.That(order.Supplier!.Name, Is.EqualTo("Test Manufacturer"));
                Assert.That(order.SupplierId, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task StockReceipt_ShouldBelongToChainProduct()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var product = new ChainProduct
                {
                    Name = "Ролкова верига 08B-1",
                    Category = new Category { Name = "Ролкови вериги" }
                };

                context.StockReceipts.Add(new StockReceipt
                {
                    Supplier = new Supplier { Name = "Test Supplier" },
                    ChainProduct = product,
                    InvoiceNumber = "INV-002",
                    InvoiceDate = DateTime.Today,
                    QuantityMeters = 50m,
                    PurchasePricePerMeter = 10m
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var receipt = await verificationContext.StockReceipts
                .Include(r => r.ChainProduct)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(receipt.ChainProduct, Is.Not.Null);
                Assert.That(receipt.ChainProduct!.Name,
                    Is.EqualTo("Ролкова верига 08B-1"));
                Assert.That(receipt.ChainProductId, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task PurchaseOrder_ShouldBelongToChainProduct()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.PurchaseOrders.Add(new PurchaseOrder
                {
                    Supplier = new Supplier { Name = "Test Manufacturer" },
                    ChainProduct = new ChainProduct
                    {
                        Name = "Ролкова верига 08B-1",
                        Category = new Category { Name = "Ролкови вериги" }
                    },
                    QuantityMeters = 50m,
                    PurchasePricePerMeter = 10m
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var order = await verificationContext.PurchaseOrders
                .Include(p => p.ChainProduct)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(order.ChainProduct, Is.Not.Null);
                Assert.That(order.ChainProduct!.Name,
                    Is.EqualTo("Ролкова верига 08B-1"));
                Assert.That(order.ChainProductId, Is.GreaterThan(0));
            });
        }
    }
}
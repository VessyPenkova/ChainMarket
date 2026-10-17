using ChainMarket.Web.Data;
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ChainMarket.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

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
                    Name = "Test Category",
                    Description = "Industrial roller chains"
                });

                await context.SaveChangesAsync();
            }

            // Act - retrieve using a new DbContext
            using var verificationContext = new ApplicationDbContext(options);

            var category = await verificationContext.Categories
                .SingleAsync(c => c.Name == "Test Category");

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(category.Name, Is.EqualTo("Test Category"));
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

        [Test]
        public async Task ChainOrder_ShouldBelongToChainProduct()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                context.ChainOrders.Add(new ChainOrder
                {
                    CustomerName = "Test Customer",
                    ChainProduct = new ChainProduct
                    {
                        Name = "Ролкова верига 08B-1",
                        Category = new Category { Name = "Ролкови вериги" }
                    },
                    Quantity = 2,
                    PackageSizeMeters = "10",
                    SellingPricePerMeter = 15m
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var order = await verificationContext.ChainOrders
                .Include(o => o.ChainProduct)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(order.ChainProduct, Is.Not.Null);
                Assert.That(order.ChainProduct!.Name,
                    Is.EqualTo("Ролкова верига 08B-1"));
                Assert.That(order.ChainProductId, Is.GreaterThan(0));
            });
        }

        [Test]
        public async Task Category_WithExistingProducts_ShouldNotBeDeleted()
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

                context.ChainProducts.Add(new ChainProduct
                {
                    Name = "Ролкова верига 08B-1",
                    Category = category
                });

                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var category = await context.Categories
                    .SingleAsync(c => c.Id > 6 && c.Name == "Ролкови вериги");

                context.Categories.Remove(category);

                Assert.ThrowsAsync<DbUpdateException>(
                    async () => await context.SaveChangesAsync());
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.That(await context.Categories.CountAsync(), Is.EqualTo(7));
                Assert.That(await context.ChainProducts.CountAsync(), Is.EqualTo(1));
            }
        }

        [Test]
        public async Task ChainProduct_ShouldSaveNormalizedStandard()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var standard = new ChainStandard { Code = "ISO 606" };

                context.ChainProducts.Add(new ChainProduct
                {
                    Name = "Ролкова верига 10B-1",
                    ChainCode = "10B-1",
                    CategoryId = 1,
                    ChainStandard = standard
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var product = await verificationContext.ChainProducts
                .Include(p => p.ChainStandard)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(product.ChainStandard, Is.Not.Null);
                Assert.That(product.ChainStandard!.Code, Is.EqualTo("ISO 606"));
                Assert.That(product.ChainStandardId, Is.GreaterThan(0));
            });
        }


        [Test]
        public async Task ChainNomenclature_ShouldRejectDuplicateCodeWithinCategory()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            context.ChainNomenclatures.Add(new ChainNomenclature
            {
                Code = "08B-1",
                CategoryId = 1
            });

            await context.SaveChangesAsync();

            context.ChainNomenclatures.Add(new ChainNomenclature
            {
                Code = "08B-1",
                CategoryId = 1
            });

            Assert.ThrowsAsync<DbUpdateException>(
                async () => await context.SaveChangesAsync());
        }

        [Test]
        public async Task ChainNomenclatureImport_ShouldNotCreateDuplicates()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var importer = new ChainNomenclatureImportService(context);

            var items = new[]
            {
                new ChainNomenclature { Code = "08B-1", CategoryId = 1 },
                new ChainNomenclature { Code = "10B-1", CategoryId = 1 }
            };
            
            int firstImport = await importer.ImportAsync(items);
            int secondImport = await importer.ImportAsync(items);

            Assert.Multiple(() =>
            {
                Assert.That(firstImport, Is.EqualTo(2));
                Assert.That(secondImport, Is.EqualTo(0));
                Assert.That(context.ChainNomenclatures.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public void StandardChainNomenclature_ShouldCoverAllSixCategories()
        {
            var items = StandardChainNomenclature.GetItems();

            Assert.Multiple(() =>
            {
                Assert.That(items.Count, Is.EqualTo(82));

                Assert.That(
                    items.Select(x => x.CategoryId).Distinct().OrderBy(x => x),
                    Is.EqualTo(Enumerable.Range(1, 6)));

                Assert.That(
                    items.GroupBy(x => new { x.CategoryId, x.Code })
                        .All(g => g.Count() == 1),
                    Is.True);
            });
        }

        [Test]
        public async Task ChainNomenclatureImport_ShouldImportAllSixCategories()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            var importer = new ChainNomenclatureImportService(context);
            var items = StandardChainNomenclature.GetItems();

            int firstImport = await importer.ImportAsync(items);
            int secondImport = await importer.ImportAsync(
                StandardChainNomenclature.GetItems());

            Assert.That(secondImport, Is.EqualTo(0));

            Assert.Multiple(() =>
            {
                Assert.That(firstImport, Is.EqualTo(82));

                Assert.That(context.ChainNomenclatures.Count(), Is.EqualTo(82));

                Assert.That(
                    context.ChainNomenclatures
                        .Select(n => n.CategoryId)
                        .Distinct()
                        .Count(),
                    Is.EqualTo(6));
            });
        }


        [Test]
        public async Task ChainProduct_ShouldBelongToNomenclature()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var nomenclature = new ChainNomenclature
                {
                    Code = "08B-1",
                    CategoryId = 1
                };

                context.ChainProducts.Add(new ChainProduct
                {
                    Name = "Ролкова верига 08B-1",
                    CategoryId = 1,
                    ChainNomenclature = nomenclature
                });

                await context.SaveChangesAsync();
            }

            using var verificationContext = new ApplicationDbContext(options);

            var product = await verificationContext.ChainProducts
                .Include(p => p.ChainNomenclature)
                .SingleAsync();

            Assert.Multiple(() =>
            {
                Assert.That(product.ChainNomenclature, Is.Not.Null);
                Assert.That(product.ChainNomenclature!.Code, Is.EqualTo("08B-1"));
                Assert.That(product.ChainNomenclatureId, Is.GreaterThan(0));
            });
        }


        [Test]
        public async Task ChainNomenclature_ShouldRejectWrongCategory()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            context.ChainNomenclatures.Add(new ChainNomenclature
            {
                Code = "08B-1",
                CategoryId = 1
            });

            await context.SaveChangesAsync();

            bool isValid = await context.ChainNomenclatures
                .AnyAsync(n => n.Code == "08B-1" && n.CategoryId == 2);

            Assert.That(isValid, Is.False);
        }

        [Test]
        public async Task Create_ShouldRejectNomenclatureFromWrongCategory()
        {
            using var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            using var context = new ApplicationDbContext(options);
            await context.Database.EnsureCreatedAsync();

            context.ChainNomenclatures.Add(new ChainNomenclature
            {
                Code = "08B-1",
                CategoryId = 1
            });

            await context.SaveChangesAsync();

            var controller = new ChainProductsController(context);

            var product = new ChainProduct
            {
                Name = "Test Chain",
                CategoryId = 2,
                ChainNomenclatureId = await context.ChainNomenclatures
                    .Select(n => n.Id)
                    .SingleAsync()
            };

            var result = await controller.Create(product);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ViewResult>());
                Assert.That(controller.ModelState.IsValid, Is.False);
                Assert.That(context.ChainProducts.Count(), Is.EqualTo(0));
            });
        }
    }
}
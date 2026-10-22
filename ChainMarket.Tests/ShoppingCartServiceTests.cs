
using ChainMarket.Web.Models;
using ChainMarket.Web.Services;

namespace ChainMarket.Tests
{
    public class ShoppingCartServiceTests
    {
        private readonly ShoppingCartService service = new();

        [Test]
        public void AddItem_ShouldAddProductToCart()
        {
            var cart = new List<ShoppingCartItem>();

            service.AddItem(cart, new ShoppingCartItem
            {
                ChainProductId = 1,
                ProductName = "Ролкова верига 08B-1",
                SellingUnit = "m",
                Quantity = 5m,
                UnitPrice = 7.90m
            });

            Assert.Multiple(() =>
            {
                Assert.That(cart, Has.Count.EqualTo(1));
                Assert.That(cart[0].Quantity, Is.EqualTo(5m));
                Assert.That(cart[0].TotalPrice, Is.EqualTo(39.50m));
            });
        }

        [Test]
        public void AddItem_ExistingProduct_ShouldIncreaseQuantity()
        {
            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    SellingUnit = "m",
                    Quantity = 3m,
                    UnitPrice = 7.90m
                }
            };

            service.AddItem(cart, new ShoppingCartItem
            {
                ChainProductId = 1,
                SellingUnit = "m",
                Quantity = 2m,
                UnitPrice = 7.90m
            });

            Assert.Multiple(() =>
            {
                Assert.That(cart, Has.Count.EqualTo(1));
                Assert.That(cart[0].Quantity, Is.EqualTo(5m));
            });
        }

        [Test]
        public void UpdateQuantity_ShouldChangeQuantity()
        {
            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    SellingUnit = "m",
                    Quantity = 2m,
                    UnitPrice = 7.90m
                }
            };

            service.UpdateQuantity(cart, 1, 10m);

            Assert.That(cart[0].Quantity, Is.EqualTo(10m));
        }

        [Test]
        public void RemoveItem_ShouldRemoveProduct()
        {
            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem { ChainProductId = 1 },
                new ShoppingCartItem { ChainProductId = 2 }
            };

            service.RemoveItem(cart, 1);

            Assert.Multiple(() =>
            {
                Assert.That(cart, Has.Count.EqualTo(1));
                Assert.That(cart[0].ChainProductId, Is.EqualTo(2));
            });
        }

        [Test]
        public void CalculateTotal_ShouldSumProductTotals()
        {
            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    Quantity = 5m,
                    UnitPrice = 7.90m
                },
                new ShoppingCartItem
                {
                    ChainProductId = 2,
                    Quantity = 2m,
                    UnitPrice = 10m
                }
            };

            Assert.That(service.CalculateTotal(cart), Is.EqualTo(59.50m));
        }

        [Test]
        public void AddItem_PiecesWithFractionalQuantity_ShouldThrow()
        {
            var cart = new List<ShoppingCartItem>();

            Assert.Throws<ArgumentException>(() => service.AddItem(cart, new ShoppingCartItem
            {
                ChainProductId = 1,
                SellingUnit = "pcs",
                Quantity = 1.5m,
                UnitPrice = 3m
            }));
        }

        [Test]
        public void UpdateQuantity_WithZeroQuantity_ShouldThrow()
        {
            var cart = new List<ShoppingCartItem>
            {
                new ShoppingCartItem
                {
                    ChainProductId = 1,
                    SellingUnit = "m",
                    Quantity = 2m
                }
            };

            Assert.Throws<ArgumentOutOfRangeException>(() => service.UpdateQuantity(cart, 1, 0m));
        }

        [Test]
        public void AddItem_WithNegativePrice_ShouldThrow()
        {
            var cart = new List<ShoppingCartItem>();

            Assert.Throws<ArgumentOutOfRangeException>(() => service.AddItem(cart, new ShoppingCartItem
            {
                ChainProductId = 1,
                SellingUnit = "m",
                Quantity = 1m,
                UnitPrice = -5m
            }));
        }
    }
}

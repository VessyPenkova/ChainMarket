
namespace ChainMarket.Web.Models
{
    public class ShoppingCartItem
    {
        public int ChainProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string? ChainCode { get; set; }

        public string SellingUnit { get; set; } = "m";

        public decimal Quantity { get; set; } = 1m;

        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }
}

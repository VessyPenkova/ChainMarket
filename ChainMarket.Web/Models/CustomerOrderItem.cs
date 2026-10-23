using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class CustomerOrderItem
    {
        public int Id { get; set; }

        [Required]
        public int CustomerOrderId { get; set; }

        public CustomerOrder? CustomerOrder { get; set; }

        [Required]
        public int ChainProductId { get; set; }

        public ChainProduct? ChainProduct { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [RegularExpression("^(m|pcs)$")]
        public string SellingUnit { get; set; } = "m";

        [Range(0.01, double.MaxValue)]
        public decimal Quantity { get; set; }

        [Range(0, double.MaxValue)]
        public decimal UnitPrice { get; set; }

        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
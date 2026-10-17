using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class PurchaseOrder
    {
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        [Required]
        public int ChainProductId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Today;

        [Range(0.01, double.MaxValue)]
        public decimal QuantityMeters { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchasePricePerMeter { get; set; }

        public DateTime? ExpectedDeliveryDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Draft";

        [StringLength(500)]
        public string? Notes { get; set; }

        public decimal TotalWithoutVat =>
            QuantityMeters * PurchasePricePerMeter;
    }
}
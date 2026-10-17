using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class StockReceipt
    {
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        public Supplier? Supplier { get; set; }

        [Required]
        public int ChainProductId { get; set; }

        public ChainProduct? ChainProduct { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal QuantityMeters { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PurchasePricePerMeter { get; set; }

        // Bulgarian VAT rate: 20%
        public decimal VatRate { get; set; } = 0.20m;

        public decimal TotalWithoutVat =>
            QuantityMeters * PurchasePricePerMeter;

        public decimal VatAmount =>
            TotalWithoutVat * VatRate;

        public decimal TotalWithVat =>
            TotalWithoutVat + VatAmount;
    }
}
using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class ChainProduct
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        [StringLength(50)]
        public string? Standard { get; set; }

        [StringLength(50)]
        public string? ChainCode { get; set; }

        // Technical dimensions in millimetres
        public decimal PitchMm { get; set; }
        public decimal InnerWidthMm { get; set; }
        public decimal RollerDiameterMm { get; set; }

        // Product images
        public string? ImagePath { get; set; }
        public string? DrawingPath { get; set; }

        // Prices in EUR, excluding VAT
        public decimal PurchasePricePerMeter { get; set; }
        public decimal SellingPricePerMeter { get; set; }

        // Available stock in metres
        public decimal StockQuantityMeters { get; set; }

        // EU Combined Nomenclature code
        [StringLength(8)]
        public string? CnCode { get; set; }
    }
}
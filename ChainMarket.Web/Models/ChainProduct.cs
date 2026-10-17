
using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class ChainProduct
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        // Standards
        [StringLength(50)]
        public string? Standard { get; set; }

        public int? ChainStandardId { get; set; }
        public ChainStandard? ChainStandard { get; set; }

        // Technical nomenclature
        public int? ChainNomenclatureId { get; set; }
        public ChainNomenclature? ChainNomenclature { get; set; }

        [StringLength(50)]
        public string? ChainCode { get; set; }

        // Number of chain strands: 1, 2 or 3
        [Range(1, 3)]
        public int? NumberOfStrands { get; set; }

        // Technical dimensions in millimetres
        public decimal PitchMm { get; set; }
        public decimal InnerWidthMm { get; set; }
        public decimal RollerDiameterMm { get; set; }

        // Product images and drawings
        public string? ImagePath { get; set; }
        public string? DrawingPath { get; set; }

        // Manufacturer reference
        // Nullable until the Manufacturer entity is implemented
        public int? ManufacturerId { get; set; }

        // Product availability in catalogue
        public bool IsActive { get; set; } = true;

        // Selling unit: m = metre, pcs = piece
        [Required]
        [RegularExpression("^(m|pcs)$")]
        [StringLength(10)]
        public string SellingUnit { get; set; } = "m";

        // Reference prices in EUR, excluding VAT, per SellingUnit
        [Range(0, double.MaxValue)]
        public decimal? PurchasePricePerUnit { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SellingPricePerUnit { get; set; }

        // Available stock in SellingUnit
        [Range(0, double.MaxValue)]
        public decimal StockQuantity { get; set; }

        // Existing prices in EUR, excluding VAT
        // Retained for compatibility and data migration
        public decimal PurchasePricePerMeter { get; set; }
        public decimal SellingPricePerMeter { get; set; }

        // Existing stock in metres
        // Retained for compatibility and data migration
        public decimal StockQuantityMeters { get; set; }

        // EU Combined Nomenclature code
        [StringLength(8)]
        public string? CnCode { get; set; }
    }
}

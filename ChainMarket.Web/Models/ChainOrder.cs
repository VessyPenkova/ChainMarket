using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class ChainOrder
    {
        public int Id { get; set; }

        [Required]
        public int ChainProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; } = 1;

        // 1 = by metre; 5, 10, 50 = package sizes in metres
        [RegularExpression("^(1|5|10|50)$")]
        public string PackageSizeMeters { get; set; } = "1";

        [Range(0, double.MaxValue)]
        public decimal SellingPricePerMeter { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Today;

        // Calculated quantities and prices
        public decimal TotalMeters =>
            Quantity * int.Parse(PackageSizeMeters);

        public decimal TotalWithoutVat =>
            TotalMeters * SellingPricePerMeter;

        public decimal VatAmount =>
            TotalWithoutVat * 0.20m;

        public decimal TotalWithVat =>
            TotalWithoutVat + VatAmount;
    }
}
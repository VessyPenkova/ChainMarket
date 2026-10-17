using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class ChainNomenclature
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Required]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        public int? ChainStandardId { get; set; }

        public ChainStandard? ChainStandard { get; set; }

        [StringLength(150)]
        public string? Description { get; set; }

        public decimal? PitchMm { get; set; }

        public decimal? InnerWidthMm { get; set; }

        public decimal? RollerDiameterMm { get; set; }
    }
}
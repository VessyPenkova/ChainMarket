using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class ChainStandard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Description { get; set; }
    }
}
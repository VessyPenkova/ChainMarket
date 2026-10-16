using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(20)]
        public string? VatNumber { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [StringLength(300)]
        public string? Address { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class CustomerOrder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string CustomerType { get; set; } = "Individual";

        [StringLength(200)]
        public string? CompanyName { get; set; }

        [StringLength(13)]
        public string? CompanyEik { get; set; }

        [StringLength(15)]
        public string? CompanyVatNumber { get; set; }

        [StringLength(300)]
        public string? BillingAddress { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalWithoutVat { get; set; }

        public decimal VatAmount { get; set; }

        public decimal TotalWithVat { get; set; }
    }
}

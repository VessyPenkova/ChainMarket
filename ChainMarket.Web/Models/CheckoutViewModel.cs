
using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class CheckoutViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Моля, изберете тип клиент.")]
        [RegularExpression("^(Individual|Company)$", ErrorMessage = "Невалиден тип клиент.")]
        [Display(Name = "Тип клиент")]
        public string CustomerType { get; set; } = "Individual";


        [Required(ErrorMessage = "Моля, въведете име.")]
        [StringLength(150)]
        [Display(Name = "Име и фамилия")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете имейл адрес.")]
        [EmailAddress(ErrorMessage = "Невалиден имейл адрес.")]
        [Display(Name = "Имейл")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете телефон.")]
        [Phone(ErrorMessage = "Невалиден телефонен номер.")]
        [Display(Name = "Телефон")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Моля, въведете адрес за доставка.")]
        [StringLength(300)]
        [Display(Name = "Адрес за доставка")]
        public string DeliveryAddress { get; set; } = string.Empty;


        [StringLength(200)]
        [Display(Name = "Име на фирмата")]
        public string? CompanyName { get; set; }

        [StringLength(13)]
        [Display(Name = "ЕИК / БУЛСТАТ")]
        public string? CompanyEik { get; set; }

        [StringLength(15)]
        [Display(Name = "ДДС номер")]
        public string? CompanyVatNumber { get; set; }

        [StringLength(300)]
        [Display(Name = "Адрес за фактуриране")]
        public string? BillingAddress { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (CustomerType != "Company")
                yield break;

            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                yield return new ValidationResult(
                    "Моля, въведете име на фирмата.",
                    new[] { nameof(CompanyName) });
            }

            if (string.IsNullOrWhiteSpace(CompanyEik))
            {
                yield return new ValidationResult(
                    "Моля, въведете ЕИК / БУЛСТАТ.",
                    new[] { nameof(CompanyEik) });
            }

            if (string.IsNullOrWhiteSpace(BillingAddress))
            {
                yield return new ValidationResult(
                    "Моля, въведете адрес за фактуриране.",
                    new[] { nameof(BillingAddress) });
            }
        }
    }
}

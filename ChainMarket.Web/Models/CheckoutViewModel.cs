
using System.ComponentModel.DataAnnotations;

namespace ChainMarket.Web.Models
{
    public class CheckoutViewModel
    {
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
    }
}

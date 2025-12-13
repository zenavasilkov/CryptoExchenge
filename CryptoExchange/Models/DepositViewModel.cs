using System.ComponentModel.DataAnnotations;

namespace CryptoExchange.Models
{
    public class DepositViewModel
    {

        [Required(ErrorMessage = "Введите номер карты")]
        [Display(Name = "Номер карты")]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "Неверный формат номера карты")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Введите срок дейстивия карты")]
        [Display(Name = "Срок действия (MM/YYYY)")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$", ErrorMessage = "Формат должен быть MM/YYYY")]
        public string Expiry {  get; set; }

        [Required(ErrorMessage = "Введите CVV")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV должен содержать 3 цифры")]
        [Display(Name = "CVV")] 
        public string CVV { get; set; }

        [Required(ErrorMessage = "Введите сумму попосления")]
        [Range(0.01, 1_000_000_000, ErrorMessage = "Сумма депозита должна быть больше 0.01$")]
        [Display(Name = "Сумма депозита")]
        public double Amount { get; set; }

        public string CurrencyCode = "USDT";
    }
}

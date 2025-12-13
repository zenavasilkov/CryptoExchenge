using System.ComponentModel.DataAnnotations;

namespace CryptoExchange.Models
{
    public class WithdrawViewModel
    {
        [Required(ErrorMessage = "Введите номер карты")]
        [Display(Name = "Номер карты")]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$", ErrorMessage = "Неверный формат номера карты")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Введите срок дейстивия карты")]
        [Display(Name = "Срок действия (MM/YYYY)")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{4}$", ErrorMessage = "Формат должен быть MM/YYYY")]
        public string Expiry { get; set; } 

        [Required(ErrorMessage = "Введите сумму вывода средств")]
        [Range(0.01, 1_000_000_000, ErrorMessage = "Сумма вывода должна быть больше 0.01$")]
        [Display(Name = "Сумма вывода")]
        public double Amount { get; set; }

        public string CurrencyCode = "USDT";
    }
}

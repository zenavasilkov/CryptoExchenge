using System.ComponentModel.DataAnnotations;

namespace CryptoExchange.Models
{
    public class CreateOrderViewModel
    {
        public int CurrencyID { get; set; }
          
        public string CurrencyShortName { get; set; }

        [Display(Name = "Тип ордера")]
        public bool IsSellOrder { get; set; }

        [Required(ErrorMessage = "Укажите цену")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Цена должна быть положительным числом")]
        public double ExchangeRate { get; set; }

        [Required(ErrorMessage = "Укажите количество")]
        [Range(0.0000001, double.MaxValue, ErrorMessage = "Количество должно быть положительным")]
        public double Amount { get; set; }
    }
}

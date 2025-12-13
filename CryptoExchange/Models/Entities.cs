using System.ComponentModel.DataAnnotations;

namespace CryptoExchange.Models
{
    public class User
    {
        [Key] public int ID { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public string PasswordHash { get; set; }

        public User(string userName, string passwordHash)
        {
            UserName = userName;
            PasswordHash = passwordHash;
        }
    }

    public class CryptoCurrency
    {
        [Key] public int ID { set; get; }
        [Required] public string Name { get; set; }
        [Required] public string ShortName { get; set; }
        [Required] public string Symbol { get; set; }
        [Required] public string Description { get; set; } 
    }

    public class Wallet
    {
        [Key] public int ID { set; get; }
        [Required] public int UserID { set; get; }
        [Required] public int CurrencyID { set; get; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Balance { get; set; } = 0;

        public Wallet(int userID, int currencyID)
        {
            UserID = userID;
            CurrencyID = currencyID;
        }

        public User User { get; set; }
        public CryptoCurrency Currency { get; set; }
    }

    public class Order
    {
        [Key] public int ID { get; set; }
        [Required] public int WalletID { get; set; }
        [Required] public int CurrencyID { get; set; }
        [Required] public bool IsSellOrder { get; set; }
        [Required] public double Amount { get; set; }
        [Required] public double ExchangeRate { get; set; }
        [Required] public DateTime CreatedAt { get; set; }
        [Required] public bool isFilled { get; set; } = false;
         
        public Wallet Wallet { get; set; }
        public CryptoCurrency Currency { get; set; }
    }

    public class Trade
    {
        [Key] public int ID { get; set; }
        [Required] public int BuyOrderID { get; set; }
        [Required] public int SellOrderID { get; set; }
        [Required] public double Amount { get; set; }
        [Required] public DateTime Timestamp { get; set; }
          
        public Order BuyOrder { get; set; }
        public Order SellOrder { get; set; }
    }

    public class MarketPriceCache
    {
        [Key]
        public int CurrencyID { get; set; }

        public double PriceInUSDT { get; set; }

        public DateTime LastUpdated { get; set; }
    }

}

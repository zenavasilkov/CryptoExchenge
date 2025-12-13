namespace CryptoExchange.Models
{
    public class CurrencyMarketViewModel
    {
        public int CurrencyID { get; set; }
        public string Symbol { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public double PriceInUSDT { get; set; } 
    }
}

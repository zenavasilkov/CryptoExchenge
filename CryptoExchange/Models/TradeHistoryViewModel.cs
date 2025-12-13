namespace CryptoExchange.Models
{
    public class TradeHistoryViewModel
    {
        public string Type { get; set; }
        public string Currency { get; set; }
        public double Amount { get; set; }
        public double ExchangeRate { get; set; }
        public double TotalInUSDT { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

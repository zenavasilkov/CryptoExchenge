using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoExchange.Models
{
    public class DBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CryptoCurrency> CryptoCurrencies { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<MarketPriceCache> MarketPriceCaches { get; set; }

        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserID);

            modelBuilder.Entity<Wallet>()
                .HasOne(w => w.Currency)
                .WithMany()
                .HasForeignKey(w =>w.CurrencyID); 
            
            modelBuilder.Entity<Order>().
                HasOne(o => o.Wallet).
                WithMany().
                HasForeignKey(o => o.WalletID);
              
            modelBuilder.Entity<Trade>().
                HasOne(t => t.BuyOrder).
                WithMany().
                HasForeignKey(t => t.BuyOrderID);

            modelBuilder.Entity<Trade>().
                HasOne(t => t.SellOrder).
                WithMany().
                HasForeignKey(t => t.SellOrderID);

            modelBuilder.Entity<CryptoCurrency>().HasData(
                new CryptoCurrency { ID = 1, Name = "Bitcoin", ShortName = "BTC", Symbol = "₿", Description = "Самая известная криптовалюта" },
                new CryptoCurrency { ID = 2, Name = "Ethereum", ShortName = "ETH", Symbol = "Ξ", Description = "Смарт-контракты" },
                new CryptoCurrency { ID = 3, Name = "Tether", ShortName = "USDT", Symbol = "$", Description = "Стейблкоин к доллару" },
                new CryptoCurrency { ID = 4, Name = "BNB", ShortName = "BNB", Symbol = "ⓑ", Description = "Binance Coin" },
                new CryptoCurrency { ID = 5, Name = "Solana", ShortName = "SOL", Symbol = "◎", Description = "Быстрый блокчейн" },
                new CryptoCurrency { ID = 6, Name = "Cardano", ShortName = "ADA", Symbol = "₳", Description = "Экологичная криптовалюта" },
                new CryptoCurrency { ID = 7, Name = "XRP", ShortName = "XRP", Symbol = "✕", Description = "Мгновенные платежи" },
                new CryptoCurrency { ID = 8, Name = "Dogecoin", ShortName = "DOGE", Symbol = "Ð", Description = "Мем-валюта" },
                new CryptoCurrency { ID = 9, Name = "Polkadot", ShortName = "DOT", Symbol = "●", Description = "Мульти-блокчейн" },
                new CryptoCurrency { ID = 10, Name = "Litecoin", ShortName = "LTC", Symbol = "Ł", Description = "Серебро к золоту BTC" },
                new CryptoCurrency { ID = 11, Name = "Chainlink", ShortName = "LINK", Symbol = "🔗", Description = "Оракулы для блокчейна" },
                new CryptoCurrency { ID = 12, Name = "Avalanche", ShortName = "AVAX", Symbol = "🗻", Description = "Высокая производительность" },
                new CryptoCurrency { ID = 13, Name = "Stellar", ShortName = "XLM", Symbol = "*", Description = "Дешевые международные переводы" },
                new CryptoCurrency { ID = 14, Name = "Monero", ShortName = "XMR", Symbol = "ɱ", Description = "Анонимные транзакции" },
                new CryptoCurrency { ID = 15, Name = "Tron", ShortName = "TRX", Symbol = "T", Description = "Контент-платформа" },
                new CryptoCurrency { ID = 16, Name = "EOS", ShortName = "EOS", Symbol = "ε", Description = "Смарт-контракты с высокой скоростью" },
                new CryptoCurrency { ID = 17, Name = "VeChain", ShortName = "VET", Symbol = "V", Description = "Цепочка поставок" },
                new CryptoCurrency { ID = 18, Name = "Aave", ShortName = "AAVE", Symbol = "Æ", Description = "Децентрализованные финансы" },
                new CryptoCurrency { ID = 19, Name = "Cosmos", ShortName = "ATOM", Symbol = "⚛", Description = "Взаимодействие блокчейнов" },
                new CryptoCurrency { ID = 20, Name = "Uniswap", ShortName = "UNI", Symbol = "🦄", Description = "Децентрализованная биржа" },
                new CryptoCurrency { ID = 21, Name = "Maker", ShortName = "MKR", Symbol = "M", Description = "Стейблкоин Dai" },
                new CryptoCurrency { ID = 22, Name = "Algorand", ShortName = "ALGO", Symbol = "A", Description = "Масштабируемость и безопасность" },
                new CryptoCurrency { ID = 23, Name = "Zcash", ShortName = "ZEC", Symbol = "ⓩ", Description = "Конфиденциальность" },
                new CryptoCurrency { ID = 24, Name = "Tezos", ShortName = "XTZ", Symbol = "ꜩ", Description = "Автоматическое обновление протокола" },
                new CryptoCurrency { ID = 25, Name = "NEO", ShortName = "NEO", Symbol = "🅝", Description = "Китайский Ethereum" }
    );
        }
    }
}

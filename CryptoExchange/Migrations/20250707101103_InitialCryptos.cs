using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CryptoExchange.Migrations
{
    /// <inheritdoc />
    public partial class InitialCryptos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CryptoCurrencies",
                columns: new[] { "ID", "Description", "Name", "ShortName", "Symbol" },
                values: new object[,]
                {
                    { 1, "Самая известная криптовалюта", "Bitcoin", "BTC", "₿" },
                    { 2, "Смарт-контракты", "Ethereum", "ETH", "Ξ" },
                    { 3, "Стейблкоин к доллару", "Tether", "USDT", "$" },
                    { 4, "Binance Coin", "BNB", "BNB", "ⓑ" },
                    { 5, "Быстрый блокчейн", "Solana", "SOL", "◎" },
                    { 6, "Экологичная криптовалюта", "Cardano", "ADA", "₳" },
                    { 7, "Мгновенные платежи", "XRP", "XRP", "✕" },
                    { 8, "Мем-валюта", "Dogecoin", "DOGE", "Ð" },
                    { 9, "Мульти-блокчейн", "Polkadot", "DOT", "●" },
                    { 10, "Серебро к золоту BTC", "Litecoin", "LTC", "Ł" },
                    { 11, "Оракулы для блокчейна", "Chainlink", "LINK", "🔗" },
                    { 12, "Высокая производительность", "Avalanche", "AVAX", "🗻" },
                    { 13, "Дешевые международные переводы", "Stellar", "XLM", "*" },
                    { 14, "Анонимные транзакции", "Monero", "XMR", "ɱ" },
                    { 15, "Контент-платформа", "Tron", "TRX", "T" },
                    { 16, "Смарт-контракты с высокой скоростью", "EOS", "EOS", "ε" },
                    { 17, "Цепочка поставок", "VeChain", "VET", "V" },
                    { 18, "Децентрализованные финансы", "Aave", "AAVE", "Æ" },
                    { 19, "Взаимодействие блокчейнов", "Cosmos", "ATOM", "⚛" },
                    { 20, "Децентрализованная биржа", "Uniswap", "UNI", "🦄" },
                    { 21, "Стейблкоин Dai", "Maker", "MKR", "M" },
                    { 22, "Масштабируемость и безопасность", "Algorand", "ALGO", "A" },
                    { 23, "Конфиденциальность", "Zcash", "ZEC", "ⓩ" },
                    { 24, "Автоматическое обновление протокола", "Tezos", "XTZ", "ꜩ" },
                    { 25, "Китайский Ethereum", "NEO", "NEO", "🅝" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CryptoCurrencies",
                keyColumn: "ID",
                keyValue: 25);
        }
    }
}

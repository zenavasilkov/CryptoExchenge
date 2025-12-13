using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoExchange.Migrations
{
    /// <inheritdoc />
    public partial class NewOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CryptoCurrencies_QuoteCurrencyID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_QuoteCurrencyID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "QuoteCurrencyID",
                table: "Orders");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuoteCurrencyID",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_QuoteCurrencyID",
                table: "Orders",
                column: "QuoteCurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CryptoCurrencies_QuoteCurrencyID",
                table: "Orders",
                column: "QuoteCurrencyID",
                principalTable: "CryptoCurrencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

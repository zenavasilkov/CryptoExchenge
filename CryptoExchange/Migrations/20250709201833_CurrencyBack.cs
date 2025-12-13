using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoExchange.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyBack : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyID",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CurrencyID",
                table: "Orders",
                column: "CurrencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_CryptoCurrencies_CurrencyID",
                table: "Orders",
                column: "CurrencyID",
                principalTable: "CryptoCurrencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_CryptoCurrencies_CurrencyID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CurrencyID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CurrencyID",
                table: "Orders");
        }
    }
}

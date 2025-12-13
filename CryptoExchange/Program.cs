using Microsoft.EntityFrameworkCore;
using CryptoExchange.Models; 

namespace CryptoExchange
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DBContext>(ops => ops.
                UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication("Cookies")
                .AddCookie("Cookies", ops =>  {
                    ops.LoginPath = "/Home/Index";
                    ops.AccessDeniedPath = "/Account/AccessDenied";
                });



            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DBContext>();
                InitializeMarketPriceChache(context).Wait();
            }
             

            if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.MapControllers();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
             
            app.Run();
        }
         
        private static async Task InitializeMarketPriceChache(DBContext _context)
        { 
            var cryptos = await _context.CryptoCurrencies.ToListAsync();
            var caches = await _context.MarketPriceCaches.ToListAsync();
            if (caches.Count == cryptos.Count) return;

            foreach (var crypto in cryptos)
            {
                var cach = _context.MarketPriceCaches.FirstOrDefault(m => m.CurrencyID == crypto.ID);
                if (cach == null)
                {
                    await _context.MarketPriceCaches.AddAsync(new MarketPriceCache()
                    {
                        CurrencyID = crypto.ID,
                        PriceInUSDT = 0,
                        LastUpdated = DateTime.UtcNow
                    });
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

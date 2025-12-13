using CryptoExchange.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace CryptoExchange.Controllers
{
    [Authorize]
    public class TradeController : Controller
    {
        private readonly DBContext _context;
        private string USDT = "USDT";

        public TradeController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userID = GetCurrentUserId();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.ID == userID);

            if (user == null) return RedirectToAction("Index", "Home");


            var usdt = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == USDT);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == userID && w.CurrencyID == usdt!.ID);
             
            return View(new TradePageViewModel
            {
                UserName = user.UserName,
                UsdtBalance = wallet?.Balance ?? 0,
                Deposit = new DepositViewModel()
            });
        }

        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_DepositForm", model);

            int userID = GetCurrentUserId();

            var usdt = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == USDT);
            if (usdt == null)  return BadRequest("USDT currency not found.");

            int usdtID = usdt.ID;

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == userID && w.CurrencyID == usdtID);

            if (wallet == null)
            {
                wallet = new Wallet(userID, usdtID);
                await _context.Wallets.AddAsync(wallet); 
            }

            wallet.Balance += model.Amount;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Trade");
        }

        public async Task<IActionResult> Withdraw(WithdrawViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_WithdrawForm", model);
            int userID = GetCurrentUserId();

            var usdt = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == USDT);
            if (usdt == null) return BadRequest("USDT currency not found.");

            int usdtID = usdt.ID;

            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == userID && w.CurrencyID == usdtID);
            if (wallet == null || wallet.Balance < model.Amount)
            {
                ModelState.AddModelError("", "Недостаточно средств для вывода.");
                return PartialView("_WithdrawForm", model);
            }

            wallet.Balance -= model.Amount;
            await _context.SaveChangesAsync();
             
            return RedirectToAction("Index", "Trade");
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var usdt = _context.CryptoCurrencies.FirstOrDefault(c => c.ShortName == USDT);

            var list = await (from crypto in _context.CryptoCurrencies
                              where crypto.ShortName != usdt!.ShortName
                              join cache in _context.MarketPriceCaches
                              on crypto.ID equals cache.CurrencyID into cryptoCacheGroup
                              from cache in cryptoCacheGroup.DefaultIfEmpty()
                              select new CurrencyMarketViewModel
                              {
                                  CurrencyID = crypto.ID,
                                  Symbol = crypto.Symbol,
                                  ShortName = crypto.ShortName,
                                  Name = crypto.Name,
                                  PriceInUSDT = cache != null ? cache.PriceInUSDT : 0
                              })
                  .OrderByDescending(c => c.PriceInUSDT)
                  .ToListAsync();
            return PartialView("Dashboard", list);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder(string shortName)
        {
            var currency = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == shortName);

            if (currency == null) return Content("Криптовалюта не найдена");

            var model = new CreateOrderViewModel
            {
                CurrencyID = currency.ID, 
                CurrencyShortName = currency.ShortName,
                IsSellOrder = false, 
            };
            return PartialView("CreateOrder", model);
        }

        public async Task<IActionResult> SubmitOrder(CreateOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                string[] numericFields = { "ExchangeRate", "Amount" };

                var customMessages = new Dictionary<string, string>
                {
                    { "ExchangeRate", "Введите корректную цену." },
                    { "Amount", "Введите корректное количество." }
                };
                
                foreach (var field in numericFields)
                {
                    if (ModelState[field]?.Errors.Any(e => e.ErrorMessage.Contains("invalid")) == true)
                    {
                        ModelState[field].Errors.Clear();
                        ModelState.AddModelError(field, customMessages[field]);
                    }
                }
                return PartialView("CreateOrder", model);
            }

            int userId = GetCurrentUserId();

            var usdt = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == USDT);
            var currency = await _context.CryptoCurrencies.FindAsync(model.CurrencyID);
            if (currency == null || usdt == null)
            {
                ModelState.AddModelError("", "Валюта не найдена");
                return PartialView("CreateOrder", model);
            }

            Wallet? walletToUse = null;

            if (model.IsSellOrder)
            { 
                walletToUse = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == userId && w.CurrencyID == model.CurrencyID);

                if (walletToUse == null || walletToUse.Balance < model.Amount)
                {
                    ModelState.AddModelError("", "Недостаточно средств на кошельке для продажи");
                    return PartialView("CreateOrder", model);
                }
                walletToUse.Balance -= model.Amount;
            }
            else
            { 
                walletToUse = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == userId && w.CurrencyID == usdt.ID);

                double totalCost = model.Amount * model.ExchangeRate;

                if (walletToUse == null || walletToUse.Balance < totalCost)
                {
                    ModelState.AddModelError("", "Недостаточно USDT для покупки");
                    return PartialView("CreateOrder", model);
                }
                walletToUse.Balance -= totalCost;
            }
              
            var order = new Order()
            {
                WalletID = walletToUse.ID, 
                CurrencyID = model.CurrencyID,
                IsSellOrder = model.IsSellOrder,
                Amount = model.Amount,
                ExchangeRate = model.ExchangeRate,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order); 
            await _context.SaveChangesAsync();
            await UpdateMarketPrice(order.CurrencyID);
             
            await MatchingEngine(order);

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, redirectUrl = Url.Action("MyOrders", "Trade") });
            }
            else
            {
                return RedirectToAction("MyOrders");
            }
        }

        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            int userId = GetCurrentUserId();

            var orders = await _context.Orders
                .Include(o => o.Wallet).ThenInclude(w => w.Currency)
                .Include(o => o.Currency)
                .Where(o => o.Wallet.UserID == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return PartialView("MyOrders", orders);
        }

        [HttpGet]
        public async Task<IActionResult> TradeHistory()
        {
            int userID = GetCurrentUserId();

            var trades = await _context.Trades
                .Include(t => t.BuyOrder).ThenInclude(o => o.Wallet)
                .Include(t => t.SellOrder).ThenInclude(o => o.Wallet)
                .Include(t => t.BuyOrder).ThenInclude(o => o.Wallet.Currency)
                .Include(t => t.SellOrder).ThenInclude(o => o.Wallet.Currency)
                .Where(t => t.BuyOrder.Wallet.UserID == userID || t.SellOrder.Wallet.UserID == userID)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            var result = trades.Select(t =>
            {
                bool isBuyer = t.BuyOrder.Wallet.UserID == userID;
                var order = isBuyer ? t.BuyOrder : t.SellOrder;
                var currency = order.Currency.ShortName;

                return new TradeHistoryViewModel() 
                {
                    Type = isBuyer ? "Покупка" : "Продажа",
                    Currency = currency,
                    Amount = t.Amount,
                    ExchangeRate = order.ExchangeRate,
                    TotalInUSDT = t.Amount * order.ExchangeRate,
                    Timestamp = t.Timestamp
                };

            }).ToList();

            return PartialView("TradeHistory", result);
        }

        public async Task<IActionResult> MyWallets()
        {
            int userID = GetCurrentUserId();

            var wallets = await _context.Wallets
                .Include(w => w.Currency)
                .Where(w => w.UserID == userID && w.Currency.ShortName != USDT) 
                .OrderByDescending(w => w.Balance)
                .ToListAsync();

            var model = wallets.Select(w => new MyCryptoViewModel()
            {
                Symbol = w.Currency.Symbol,
                ShortName = w.Currency.ShortName,
                Name = w.Currency.Name,
                Balance = w.Balance.ToString()
            }).ToList();

            return PartialView("MyWallets", model);
        }

        private int GetCurrentUserId()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userIdClaim = User.FindFirst("UserID");
                if (userIdClaim != null) return int.Parse(userIdClaim.Value);
            }

            throw new Exception("Пользователь не авторизован.");
        }

        private async Task MatchingEngine(Order order)
        {
            if (order == null || order.isFilled) return;

            double remainingAmount = order.Amount;
             
            var usdt = await _context.CryptoCurrencies.FirstOrDefaultAsync(c => c.ShortName == USDT);
            if (usdt == null) return;

            int usdtCurrencyId = usdt.ID;

            int baseCurrencyId = order.CurrencyID;

            var query = _context.Orders
                .Include(o => o.Wallet)
                .Where(o =>
                    o.IsSellOrder != order.IsSellOrder &&
                    o.CurrencyID == baseCurrencyId &&
                    !o.isFilled);

            if (order.IsSellOrder)
            {
                query = query
                    .Where(o => o.ExchangeRate >= order.ExchangeRate)
                    .OrderBy(o => o.ExchangeRate)
                    .ThenBy(o => o.CreatedAt);
            }
            else
            {
                query = query
                    .Where(o => o.ExchangeRate <= order.ExchangeRate)
                    .OrderBy(o => o.ExchangeRate)
                    .ThenBy(o => o.CreatedAt);
            }

            var oppositeOrders = await query.ToListAsync();

            foreach (var match in oppositeOrders)
            {
                if (remainingAmount <= 0) break;

                double dealAmount = Math.Min(remainingAmount, match.Amount);
                double dealPrice = match.ExchangeRate;
                double totalCost = dealAmount * dealPrice;
                 
                var buyerOrder = order.IsSellOrder ? match : order;
                var sellerOrder = order.IsSellOrder ? order : match;
                 
                var buyerWallet = await _context.Wallets.FirstOrDefaultAsync(w =>
                    w.UserID == buyerOrder.Wallet.UserID && w.CurrencyID == baseCurrencyId);
                if (buyerWallet == null)
                {
                    buyerWallet = new Wallet(buyerOrder.Wallet.UserID, baseCurrencyId) { Balance = 0 };
                    await _context.Wallets.AddAsync(buyerWallet);
                }
                buyerWallet.Balance += dealAmount;

                var buyerUSDTWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserID == buyerWallet.UserID && w.Currency.ShortName == USDT); 
                if(buyerUSDTWallet == null) 
                { 
                    buyerUSDTWallet = new Wallet(buyerWallet.UserID, usdt.ID) { Balance = 0 };
                    await _context.Wallets.AddAsync(buyerUSDTWallet);
                }
                  
                buyerUSDTWallet.Balance += (buyerOrder.ExchangeRate - sellerOrder.ExchangeRate) * dealAmount;

                var sellerUSDTWallet = await _context.Wallets.FirstOrDefaultAsync(w =>
                    w.UserID == sellerOrder.Wallet.UserID && w.CurrencyID == usdtCurrencyId);
                if (sellerUSDTWallet == null)
                {
                    sellerUSDTWallet = new Wallet(sellerOrder.Wallet.UserID, usdtCurrencyId) { Balance = 0 };
                    await _context.Wallets.AddAsync(sellerUSDTWallet);
                }
                sellerUSDTWallet.Balance += totalCost;
                 
                match.Amount -= dealAmount;
                if (match.Amount <= 0) match.isFilled = true;

                remainingAmount -= dealAmount;
                if (remainingAmount <= 0) order.isFilled = true;

                var trade = new Trade
                {
                    BuyOrderID = buyerOrder.ID,
                    SellOrderID = sellerOrder.ID,
                    Amount = dealAmount,
                    Timestamp = DateTime.UtcNow
                };

                await _context.Trades.AddAsync(trade); 
                await _context.SaveChangesAsync();
            }

            order.Amount = remainingAmount;
            order.isFilled = remainingAmount <= 0;

            await _context.SaveChangesAsync();
        }

        private async Task UpdateMarketPrice(int currencyID)
        {
            var lastTrades = await _context.Trades
                .Include(t => t.BuyOrder).ThenInclude(o => o.Wallet).ThenInclude(w => w.Currency)
                .Include(t => t.SellOrder).ThenInclude(o => o.Wallet).ThenInclude(w => w.Currency)
                .Where(t => t.BuyOrder.CurrencyID == currencyID || t.SellOrder.CurrencyID == currencyID)
                .OrderByDescending(t => t.Timestamp)
                .Take(10)
                .ToListAsync();

            if (!lastTrades.Any()) return;

            double averagePrice = lastTrades.Average( t=> t.BuyOrder.ExchangeRate );
            var cacheEntry = await _context.MarketPriceCaches.FirstOrDefaultAsync(m => m.CurrencyID == currencyID);

            if (cacheEntry == null)
            {
                cacheEntry = new MarketPriceCache
                {
                    CurrencyID = currencyID,
                    PriceInUSDT = averagePrice,
                    LastUpdated = DateTime.UtcNow
                };
                await _context.MarketPriceCaches.AddAsync(cacheEntry);
            }
            else
            {
                cacheEntry.PriceInUSDT = averagePrice;
                cacheEntry.LastUpdated = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        } 

    }
}

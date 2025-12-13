using Microsoft.AspNetCore.Mvc;

namespace CryptoExchange.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
    }
}

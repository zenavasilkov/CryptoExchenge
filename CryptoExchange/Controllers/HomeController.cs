
using Microsoft.AspNetCore.Mvc;

namespace CryptoExchange.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

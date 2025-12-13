using CryptoExchange.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
using BCrypt;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CryptoExchange.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBContext _context;

        public AccountController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RegisterForm()
        {
            var model = new AccountFormViewModel
            {
                Login = new LoginViewModel(),
                Register = new RegisterViewModel(),
                ActiveTab = "login"
            };
            return PartialView("_AccountForm", model);
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var formModel = new AccountFormViewModel
                    {
                        Register = model,
                        ActiveTab = "register"
                    };
                    return PartialView("_AccountForm", formModel);
                }

                return View(model);
            }

            var exUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Login);

            if (exUser != null)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var formModel = new AccountFormViewModel
                    {
                        Register = model,
                        ActiveTab = "register"
                    };
                    return PartialView("_AccountForm", formModel);
                }

                return View(model);
            }

            var user = new User(model.Login, HashPassword(model.Password));
             
            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Login);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserID", user.ID.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
             
            return RedirectToAction("Index", "Trade");
        } 

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var formModel = new AccountFormViewModel
                    {
                        Login = model,
                        ActiveTab = "login"
                    };
                    return PartialView("_AccountForm", formModel);
                }

                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.Login);

            if (user != null && VerifyPassword(model.Password, user.PasswordHash))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim("UserID", user.ID.ToString())
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Trade");
            }

            ModelState.AddModelError("", "Неверный логин или пароль");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var formModel = new AccountFormViewModel
                {
                    Login = model,
                    ActiveTab = "login"
                };
                return PartialView("_AccountForm", formModel);
            }

            return View(model);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        private bool VerifyPassword(string password, string hashedPassword) => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}

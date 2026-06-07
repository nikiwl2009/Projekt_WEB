using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Projekt_WEB.Data;
using Projekt_WEB.Models;

namespace Projekt_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string returnUrl = null)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewData["Error"] = "Wprowadź email i hasło.";
                return View();
            }

            var admin = _context.AdminUsers.FirstOrDefault(u => u.Email == email);
            if (admin == null)
            {
                ViewData["Error"] = "Nieprawidłowy email lub hasło.";
                return View();
            }

            var hasher = new PasswordHasher<AdminUser>();
            var result = hasher.VerifyHashedPassword(admin, admin.PasswordHash, password);
            if (result == PasswordVerificationResult.Failed)
            {
                ViewData["Error"] = "Nieprawidłowy email lub hasło.";
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, admin.Email),
        new Claim(ClaimTypes.Role, admin.Role ?? "Admin")
    };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}

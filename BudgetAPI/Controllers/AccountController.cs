using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyPersonalBudgetAPI.Controllers
{
    public class AccountController : Controller
    {
        ILogger logger;
        BudgetAPI.Interfaces.IAuthenticationService authenticationService;

        public AccountController(BudgetAPI.Interfaces.IAuthenticationService authService, ILogger logger)
        {
            authenticationService = authService;
            this.logger = logger;
        }


        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (authenticationService.VerifyUser(username, password))
            {
                logger.LogInformation("Verifying user: verification passed ");

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username)
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                
                return RedirectToAction("Index", "HomeBudget");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}


using Client.Repositories.Interface;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Client.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository repository;

        public AccountController(IAccountRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Logins()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Logins(LoginVM login)
        {
            var result = await repository.Logins(login);
            var token = result.Data;
            var claims = ExtractClaims(token);
            var getRole = "";
            /*Console.WriteLine(claims);
*/
            foreach (var claim in claims)
            {
                if (claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                {
                    getRole = claim.Value;
                    /* Console.WriteLine($"Claim Type: {claim.Type} - Claim Value: {claim.Value}");
                     HttpContext.Session.SetString("Role", claim.Value);*/
                }

            }
            /*            Console.WriteLine(getRole);
            */
            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            if (result.Code == 200)
            {
                HttpContext.Session.SetString("JWToken", result.Data);
                /*return RedirectToAction("LandingPage", "Home");*/
                /*var role = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role);*/
                if (getRole == "Admin")
                {
                    return RedirectToAction("Admin", "Home");
                }
                else if (getRole == "Finance")
                {
                    return RedirectToAction("Finance", "Home");
                }
                else if (getRole == "Developer")
                {
                    return RedirectToAction("Developer", "Home");
                }
                else if (getRole == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
        }

        public IEnumerable<Claim> ExtractClaims(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            return claims;
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Account/Logins");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {

            var result = await repository.Register(registerVM);
            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
                return RedirectToAction("GetAllEmployee", "Employee");
            }
            return View();
        }
    }
}


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

        [Authorize(Roles = "Admin")]
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
        public async Task<IActionResult> Logins(LoginVM loginS)
        {
            var result = await repository.Logins(loginS);

            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            else if (result.Code == 401) // Invalid email or password
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return View();
            }
            else if (result.Code == 200)
            {
                var token = result.Data;
                var claims = ExtractClaims(token);
                var getRole = "";

                foreach (var claim in claims)
                {
                    if (claim.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                    {
                        getRole = claim.Value;
                    }
                }

                HttpContext.Session.SetString("JWToken", token);

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
                    return RedirectToAction("User", "Home");
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
    }
}

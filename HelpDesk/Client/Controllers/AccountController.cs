
using Client.Repositories.Interface;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Logins(LoginVM loginVM)
        {
            var result = await repository.Logins(loginVM);
            if (result.Code == 0)
            {
                return RedirectToAction("Notfound", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }
            else if (result.Code == 200)
            {
                HttpContext.Session.SetString("JWToken", result.Data);
                if (User.IsInRole("Admin"))
                {
                    // Check if the user is already logged in as an admin
                    if (HttpContext.Session.GetString("IsAdminLoggedIn") != null)
                    {
                        // Redirect to the admin page directly
                        return RedirectToAction("Index", "Employee");
                    }
                    else
                    {
                        // Set a session variable indicating that the user is now logged in as an admin
                        HttpContext.Session.SetString("IsAdminLoggedIn", "true");
                    }
                }

                // Redirect to the appropriate page based on the user's role
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Admin", "Home"); // Redirect to the admin page
                }
                else if (User.IsInRole("User"))
                {

                    return RedirectToAction("Index", "Home"); // Redirect to the user page
                }
            }

            return View();
        }

        [HttpGet("/Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Account/Login");
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

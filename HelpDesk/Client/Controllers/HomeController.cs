using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult User()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;
            return View();
        }

        [AllowAnonymous]
        [HttpGet("/Unauthorized")]
        public IActionResult Unauthorized()
        {
            return View("401");
        }

        [AllowAnonymous]
        [HttpGet("/NotFound")]
        public IActionResult NotFound()
        {
            return View("404");
        }

        [AllowAnonymous]
        [HttpGet("/Forbidden")]
        public IActionResult Forbidden()
        {
            return View("403");
        }

        [Authorize(Roles = "Developer")]
        public IActionResult Developer()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }
        [Authorize(Roles = "Finance")]
        public IActionResult Finance()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
using Client.Models;
using Client.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository repository;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CategoryController(ICategoryRepository repository, IHttpContextAccessor httpContextAccessor)
        {
            this.repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.Gets(jwToken);
            var categories = new List<Category>();

            if (result.Data != null)
            {
                categories = result.Data.Select(e => new Category
                {
                    Guid = e.Guid,
                    CategoryName = e.CategoryName,
                }).ToList();
            }
            return View(categories);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Creates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Creates(Category category)
        {
            var result = await repository.Posts(category);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View();
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletes(Guid guid)
        {
            var result = await repository.Gets(guid);
            var complain = new Category();
            if (result.Data?.Guid is null)
            {
                return View(complain);
            }
            else
            {
                complain.Guid = result.Data.Guid;
                complain.CategoryName = result.Data.CategoryName;
            }
            return View(complain);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Remove(Guid guid)
        {
            var result = await repository.Deletes(guid);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await repository.Puts(category);
                if (result.Code == 200)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid Guid)
        {
            var result = await repository.Gets(Guid);
            var category = new Category();
            if (result.Data?.Guid is null)
            {
                return View(category);
            }
            else
            {
                category.Guid = result.Data.Guid;
                category.CategoryName = result.Data.CategoryName;
                category.CreatedDate = result.Data.CreatedDate;
                category.ModifiedDate = DateTime.Now;
            }

            return View(category);
        }
    }

}

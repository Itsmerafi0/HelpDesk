using Client.Models;
using Client.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly ISubCategoryRepository repository;

        public SubCategoryController(ISubCategoryRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Gets();
            var subcategories = new List<SubCategory>();

            if (result.Data != null)
            {
                subcategories = result.Data.Select(e => new SubCategory
                {
                    Guid = e.Guid,
                    Name = e.Name,
                    CategoryGuid = e.CategoryGuid,
                    CreatedDate = e.CreatedDate,
                    ModifiedDate = e.ModifiedDate,
                }).ToList();
            }

            return View(subcategories);
        }



        [HttpGet]
        public async Task<IActionResult> Creates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creates(SubCategory subCategory)
        {
            var result = await repository.Posts(subCategory);
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
    }
}

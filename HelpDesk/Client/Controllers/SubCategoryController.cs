using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.Gets(jwToken);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllSub()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.GetAllSub(jwToken);
            var subcategories = new List<SubCategoryDetailVM>();

            if (result.Data != null)
            {
                subcategories = result.Data.Select(e => new SubCategoryDetailVM
                {
                    Guid = e.Guid,
                    Name = e.Name,
                    CategoryName = e.CategoryName,
                    RiskLevel = e.RiskLevel
                }).ToList();
            }

            return View(subcategories);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Creates()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Creates(SubCategory subCategory)
        {
            var result = await repository.Posts(subCategory);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(GetAllSub));
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
            var complain = new SubCategory();
            if (result.Data?.Guid is null)
            {
                return View(complain);
            }
            else
            {
                complain.Guid = result.Data.Guid;
                complain.Name = result.Data.Name;
                complain.CategoryGuid = result.Data.CategoryGuid;
                complain.RiskLevel = result.Data.RiskLevel;
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
        public async Task<IActionResult> Edit(SubCategory category)
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
            var category = new SubCategory();
            if (result.Data?.Guid is null)
            {
                return View(category);
            }
            else
            {
                category.Guid = result.Data.Guid;
                category.Name = result.Data.Name;
             
                category.CreatedDate = result.Data.CreatedDate;
                category.ModifiedDate = DateTime.Now;
            }

            return View(category);
        }
    }
}


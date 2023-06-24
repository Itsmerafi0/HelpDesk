using Client.Models;
using Client.Repositories.Interface;
using Client.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ComplainController : Controller
    {
        private readonly IComplainRepository repository;

        public ComplainController(IComplainRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Gets();
            var complains = new List<Complain>();

            if (result.Data != null)
            {
                complains = result.Data.Select(e => new Complain
                {
                    Guid = e.Guid,
                    Description = e.Description,
                    Attachment = e.Attachment,
                    EmployeeGuid = e.EmployeeGuid,
                    CreatedDate = e.CreatedDate,
                    ModifiedDate = e.ModifiedDate
                }).ToList();
            }
            return View("Index", complains);
        }

        public async Task<IActionResult> Urgent(Guid Guid)
        {
            var result = await repository.Gets();
            var complains = new List<Complain>();

            if (result.Data != null)
            {
                complains = result.Data
                    .Where(e => e.RiskLevel == Risk.Urgent && e.Guid == Guid) // Filter complaints with "Urgent" risk level and specific GUID
                    .Select(e => new Complain
                    {
                        Guid = e.Guid,
                        Category = e.Category,
                        SubCategory = e.SubCategory,
                        StatusLevel = e.StatusLevel,
                        RiskLevel = e.RiskLevel,
                        Description = e.Description,
                        Attachment = e.Attachment,
                        FinishDate = e.FinishDate,
                        EmployeeGuid = e.EmployeeGuid,
                        CreatedDate = e.CreatedDate,
                        ModifiedDate = e.ModifiedDate
                    })
                    .ToList();
            }

            return View("Index", complains);
        }

        public async Task<IActionResult> Plans(Guid Guid)
        {
            var result = await repository.Gets();
            var complains = new List<Complain>();

            if (result.Data != null)
            {
                complains = result.Data
                    .Where(e => e.RiskLevel == Risk.Plans && e.Guid == Guid) // Filter complaints with "Urgent" risk level
                    .Select(e => new Complain
                    {
                        Guid = e.Guid,
                        Category = e.Category,
                        SubCategory = e.SubCategory,
                        StatusLevel = e.StatusLevel,
                        RiskLevel = e.RiskLevel,
                        Description = e.Description,
                        Attachment = e.Attachment,
                        FinishDate = e.FinishDate,
                        EmployeeGuid = e.EmployeeGuid,
                        CreatedDate = e.CreatedDate,
                        ModifiedDate = e.ModifiedDate
                    })
                    .ToList();
            }

            return View("Plans", complains);
        }

        [HttpGet]
        public async Task<IActionResult> Creates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Complain complain)
        {
            var result = await repository.Posts(complain);
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

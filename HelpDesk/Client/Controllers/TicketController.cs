using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Client.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TicketController(ITicketRepository repository, IHttpContextAccessor httprepository)
        {
            this.repository = repository;
            this._httpContextAccessor = httprepository;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.Gets(jwToken);
            var complains = new List<Ticket>();

            if (result.Data != null)
            {
                complains = result.Data.Select(e => new Ticket
                {
                    Guid = e.Guid,
                    TicketId = e.TicketId,
                    SubCategoryGuid = e.SubCategoryGuid,
                    Description = e.Description,
                    Attachment = e.Attachment,
                    EmployeeGuid = e.EmployeeGuid,
                    CreatedDate = e.CreatedDate,
                    ModifiedDate = e.ModifiedDate
                }).ToList();
            }
            return View("Index", complains);
        }

        /* public async Task<IActionResult> Urgent(Guid Guid)
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
         }*/

        /* public async Task<IActionResult> Plans(Guid Guid)
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
                         SubCategory = e.SubCategory,
                         Description = e.Description,
                         Attachment = e.Attachment,
                         EmployeeGuid = e.EmployeeGuid,
                         CreatedDate = e.CreatedDate,
                         ModifiedDate = e.ModifiedDate
                     })
                     .ToList();
             }

             return View("Plans", complains);
         }*/

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Creates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Creates(Ticket complain)
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTicketDetails()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }

        [Authorize(Roles = "Developer")]
        public async Task<IActionResult> GetAllTicketDev()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        } 


        [Authorize]
        public async Task<IActionResult> GetAllTicketUser()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }
        [Authorize(Roles = "Finance")]
        public async Task<IActionResult> GetAllTicketFinance()
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deletes(Guid guid)
        {
            var result = await repository.Gets(guid);
            var complain = new Ticket();
            if (result.Data?.Guid is null)
            {
                return View(complain);
            }
            else
            {
                complain.Guid = result.Data.Guid;
                complain.SubCategoryGuid = result.Data.SubCategoryGuid;
                complain.Description = result.Data.Description;
                complain.Attachment = result.Data.Attachment;
                complain.EmployeeGuid = result.Data.EmployeeGuid;
                complain.CreatedDate = result.Data.CreatedDate;
                complain.ModifiedDate = result.Data.ModifiedDate;
              
            }
            return View(complain);
        }

        [HttpPost]
        //   [ValidateAntiForgeryToken]
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTicketAccess()
        {

            var ticketresoVM = Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewData["EmployeeGuid"] = ticketresoVM;

            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTicketAccess(TicketResoVM ticketResoVM)
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.CreateTicket(ticketResoVM,jwToken);
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
                TempData["Success"] = $"Data has been Successfully Ticket! - {result.Message}!";
                return RedirectToAction("GetAllTicketUser", "Ticket");
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTicketReimbersment()
        {

            var ticketresoVM = Guid.Parse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier));

            ViewData["EmployeeGuid"] = ticketresoVM;

            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            ViewData["JWToken"] = jwToken;

            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTicketReimbersment(TicketResoVM ticketResoVM)
        {
            string jwToken = HttpContext.Session.GetString("JWToken") ?? "JWT is null";
            var result = await repository.CreateTicket(ticketResoVM, jwToken);
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
                TempData["Success"] = $"Data has been Successfully Ticket! - {result.Message}!";
                return RedirectToAction("GetAllTicketUser", "Ticket");
            }
            return View();
        }
    }


}


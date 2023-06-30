using Client.Models;
using Client.Repositories.Interface;
using Client.Utility;
using Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository repository;

        public TicketController(ITicketRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Gets();
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
        public async Task<IActionResult> Creates()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> GetAllTicketDetails()
        {
            return View();
        }
         public async Task<IActionResult> GetAllTicketDev()
        {
            var complainResult = await repository.GetAllTicketDev();
            var complains = new List<GetTicketForDevVM>();

            if (complainResult.Data != null)
            {
                complains = complainResult.Data.Select(c => new GetTicketForDevVM
                {
                  Guid = c.Guid,
                  TicketId = c.TicketId,
                  Requester = c.Requester,
                  Email = c.Email,
                  SubCategoryName = c.SubCategoryName,
                  Attachment = c.Attachment,
                  RiskLevel = c.RiskLevel,
                  StatusLevel = c.StatusLevel,
                  Description = c.Description,
                  ResolutionNote = c.ResolutionNote,
                  FinishDate = c.FinishDate
                }).ToList();
            }


            return View(complains);
        } 
        public async Task<IActionResult> GetAllTicketFinance()
        {
            var complainResult = await repository.GetAllTicketFinance();
            var complains = new List<GetTicketForFinance>();

            if (complainResult.Data != null)
            {
                complains = complainResult.Data.Select(c => new GetTicketForFinance
                {
                  Guid = c.Guid,
                  TicketId= c.TicketId,
                  Requester = c.Requester,
                  Email = c.Email,
                  SubCategoryName = c.SubCategoryName,
                  RiskLevel = c.RiskLevel,
                  Attachment = c.Attachment,
                  StatusLevel = c.StatusLevel,
                  Description = c.Description,
                  ResolutionNote = c.ResolutionNote,
                }).ToList();
            }


            return View(complains);
        }

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
        public async Task<IActionResult> CreateTicket()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketResoVM ticketResoVM)
        {

            var result = await repository.CreateTicket(ticketResoVM);
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
                return RedirectToAction("GetAllTicketDetail", "Ticket");
            }
            return View();
        }
    }


}


using API.Contracs;
using API.Models;
using API.Utility;
using API.ViewModel.Resolution;
using API.ViewModel.Response;
using API.ViewModel.Role;
using API.ViewModel.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class resolutionController : BaseController<Resolution, ResolutionVM>
    {
        private readonly IResolutionRepository _resolutionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper<Resolution, ResolutionVM> _mapper;
        private readonly IEmailService _emailService;
        public resolutionController(IResolutionRepository resolutionRepository,
            IMapper<Resolution, ResolutionVM> mapper, IEmailService emailService, ITicketRepository ticketRepository) : base(resolutionRepository, mapper)
        {
            _resolutionRepository = resolutionRepository;
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpPost("updatestatus")]
        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateStatus")]
        public IActionResult UpdateResolutionStatus(Guid ticketGuid, StatusLevel newStatus)
        {
            var resolution = _resolutionRepository.GetByGuid(ticketGuid);

            if (resolution == null)
            {
                return BadRequest(new ResponseVM<UpdateStatusVM>
                {
                    Code = 400,
                    Status = "Bad Request",
                    Message = "Invalid ticket guid",
                    Data = null
                });
            }
            _resolutionRepository.UpdateStatus(resolution, newStatus);
            SendEmailNotification(ticketGuid, newStatus);

            var result = new UpdateStatusVM
            {
                Guid = ticketGuid,
                Status = newStatus,
            };

            return Ok(new ResponseVM<UpdateStatusVM>
            {
                Code = 200,
                Status = "Success",
                Message = "Ticket status updated successfully.",
                Data = result
            });
        }

        private void SendEmailNotification(Guid ticketGuid, StatusLevel newStatus)
        {
            string recipient = _ticketRepository.FindEmailByComplainGuid(ticketGuid);
            string ticketId = _ticketRepository.FindIdByTicketGuid(ticketGuid);
            string subject = "Status Update";
            string htmlMessage = $"The status of your ticket with ID #{ticketId} has been changed to {newStatus}.";

            _emailService.SetEmail(recipient)
                         .SetSubject(subject)
                         .SetHtmlMessage(htmlMessage)
                         .SendEmailAsync();
        }

    }
}

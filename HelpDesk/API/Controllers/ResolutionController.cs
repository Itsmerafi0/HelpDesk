using API.Contracs;
using API.Models;
using API.Utility;
using API.ViewModel.Resolution;
using API.ViewModel.Response;
using API.ViewModel.Ticket;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResolutionController : BaseController<Resolution, ResolutionVM>
    {
        private readonly IResolutionRepository _resolutionRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper<Resolution, ResolutionVM> _mapper;
        private readonly IEmailService _emailService;
        public ResolutionController(IResolutionRepository resolutionRepository,
            IMapper<Resolution, ResolutionVM> mapper, IEmailService emailService, ITicketRepository ticketRepository) : base(resolutionRepository, mapper)
        {
            _resolutionRepository = resolutionRepository;
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateResolutionStatus(Guid complainGuid, StatusLevel newStatus)
        {
            var resolution = _resolutionRepository.GetByGuid(complainGuid);

            if (resolution == null)
            {
                return BadRequest(new ResponseVM<UpdateStatusVM>
                {
                    Code = 400,
                    Status = "Bad Request",
                    Message = "Invalid complain guid",
                    Data = null
                });
            }
            _resolutionRepository.UpdateStatus(resolution, newStatus);
            SendEmailNotification(complainGuid, newStatus);

            var result = new UpdateStatusVM
            {
                Guid = complainGuid,
                Status = newStatus,
            };

            return Ok(new ResponseVM<UpdateStatusVM>
            {
                Code = 200,
                Status = "Success",
                Message = "Complain status updated successfully.",
                Data = result
            });
        }

        private void SendEmailNotification(Guid complainGuid, StatusLevel newStatus)
        {
            string recipient = _ticketRepository.FindEmailByComplainGuid(complainGuid);
            string subject = "Status Update";
            string htmlMessage = $" The status has been changed to {newStatus}.";

            _emailService.SetEmail(recipient)
                         .SetSubject(subject)
                         .SetHtmlMessage(htmlMessage)
                         .SendEmailAsync();
        }

    }
}

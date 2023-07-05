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

        [HttpPut("UpdateStatus")]
        public IActionResult UpdateResolutionStatus(Guid resolutionGuid, StatusLevel newStatus)
        {
            var resolution = _resolutionRepository.GetByGuid(resolutionGuid);

            if (resolution == null)
            {
                return BadRequest(new ResponseVM<UpdateStatusVM>
                {
                    Code = 400,
                    Status = "Bad Request",
                    Message = "Invalid resolution guid",
                    Data = null
                });
            }
            _resolutionRepository.UpdateStatus(resolution, newStatus);
            SendEmailNotification(resolutionGuid, newStatus);

            var result = new UpdateStatusVM
            {
                ResolutionGuid = resolutionGuid,
                NewStatus = newStatus,
            };

            return Ok(new ResponseVM<UpdateStatusVM>
            {
                Code = 200,
                Status = "Success",
                Message = "Resolution status updated successfully.",
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

        [HttpPut("UpdateResolvedBy")]
        public IActionResult UpdateResolvedBy(Guid resolutionGuid, Guid resolvedBy)
        {
            var resolution = _resolutionRepository.GetByGuid(resolutionGuid);

            if (resolution == null)
            {
                return BadRequest(new ResponseVM<UpdateResolvedByVM>
                {
                    Code = 400,
                    Status = "Bad Request",
                    Message = "Invalid resolution guid",
                    Data = null
                });
            }
            _resolutionRepository.UpdateResolvedBy(resolution, resolvedBy);

            var result = new UpdateResolvedByVM
            {
                ResolutionGuid = resolutionGuid,
                ResolvedBy = resolvedBy
            };

            return Ok(new ResponseVM<UpdateResolvedByVM>
            {
                Code = 200,
                Status = "Success",
                Message = "ResolvedBy updated successfully.",
                Data = result
            });
        }

        [HttpPut("UpdateResolutionNotes")]
        public IActionResult UpdateResolutionNotes(Guid resolutionGuid, string notes)
        {
            var resolution = _resolutionRepository.GetByGuid(resolutionGuid);

            if (resolution == null)
            {
                return BadRequest(new ResponseVM<UpdateResolutionNoteVM>
                {
                    Code = 400,
                    Status = "Bad Request",
                    Message = "Invalid resolution guid",
                    Data = null
                });
            }

            _resolutionRepository.UpdateResolutionNote(resolution, notes);

            var result = new UpdateResolutionNoteVM
            {
                ResolutionGuid = resolutionGuid,
                Notes = notes
            };

            return Ok(new ResponseVM<UpdateResolutionNoteVM>
            {
                Code = 200,
                Status = "Success",
                Message = "Resolution note updated successfully.",
                Data = result
            });
        }

    }
}
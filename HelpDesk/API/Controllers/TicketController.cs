using API.Contracs;
using API.Models;
using API.Repository;
using API.Utility;
using API.ViewModel.Response;
using API.ViewModel.Ticket;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class ticketController : BaseController<Ticket, TicketVM>
{
    private readonly ITicketRepository _complainRepository;
    private readonly IMapper<Ticket, TicketVM> _mapper;


    public ticketController(ITicketRepository Complainrepository,
    IMapper<Ticket, TicketVM> mapper) : base(Complainrepository, mapper)
    {
        _mapper = mapper;
        _complainRepository = Complainrepository;

    }

    [HttpGet("ticketdetail")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllComplainDetail()
    {
        try
        {

            var results = _complainRepository.GetAllComplainDetail();
            return Ok(new ResponseVM<List<TicketDetailVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<TicketDetailVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

    }
    
    [HttpGet("ticketdetaildeveloper")]
    [Authorize(Roles = "Developer")]
    public IActionResult GetAllComplainDev()
    {
        try
        {

            var results = _complainRepository.GetAllComplainDev();
            
            return Ok(new ResponseVM<List<GetTicketForDevVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<GetTicketForDevVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
    } 
    
    [HttpGet("ticketdetailuser")]
    [Authorize(Roles ="User")]
    public IActionResult GetAllComplainUser()
    {
        try
        {

            var results = _complainRepository.GetAllComplainUser();
            
            return Ok(new ResponseVM<List<GetComplainForUserVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<GetComplainForUserVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
    }
    [HttpGet("ticketdetailfinance")]
    [Authorize(Roles = "Finance")]
    public IActionResult GetAllComplainFinance()
    {
        try
        {

            var results = _complainRepository.GetAllComplainFinance();

            return Ok(new ResponseVM<List<GetTicketForFinanceVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<GetTicketForFinanceVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
    }

    [HttpPost("createticket")]
    [Authorize]
    public IActionResult CreateReso(TicketResoVM complainresoVM)
    {
        var employeeGuid = complainresoVM.EmployeeGuid;

        var results = _complainRepository.CreateTicketResolution(complainresoVM);
        switch (results)
        {
            case 0:
                return BadRequest(new ResponseVM<TicketResoVM>
                {
                    Code = StatusCodes.Status400BadRequest,
                    Status = HttpStatusCode.BadRequest.ToString(),
                    Message = "Create Failed",
                });
            case 1:
                return Ok(new ResponseVM<TicketResoVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Create Success"
                });
        }
        return Ok(new ResponseVM<TicketResoVM>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Create succeed",
        });
    }
}


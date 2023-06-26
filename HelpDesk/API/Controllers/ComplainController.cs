using API.Contracs;
using API.Models;
using API.Repository;
using API.ViewModel.Complain;
using API.ViewModel.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class ComplainController : BaseController<Complain, ComplainVM>
{
    private readonly IComplainRepository _complainRepository;
    private readonly IMapper<Complain, ComplainVM> _mapper;

    public ComplainController(IComplainRepository Complainrepository,
    IMapper<Complain, ComplainVM> mapper) : base(Complainrepository, mapper)
    {
        _mapper = mapper;
        _complainRepository = Complainrepository;

    }

    [HttpGet("ComplainDetail")]
    public IActionResult GetAllComplainDetail()
    {
        try
        {

            var results = _complainRepository.GetAllComplainDetail();

            return Ok(new ResponseVM<List<ComplainDetailVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<ComplainDetailVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }

    }

    [HttpGet("ComplainDetailDeveloper")]
    public IActionResult GetAllComplainDev()
    {
        try
        {

            var results = _complainRepository.GetAllComplainDev();

            return Ok(new ResponseVM<List<GetComplainForDevVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<GetComplainForDevVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
    }

    [HttpGet("ComplainDetailFinance")]
    public IActionResult GetAllComplainFinance()
    {
        try
        {

            var results = _complainRepository.GetAllComplainFinance();

            return Ok(new ResponseVM<List<GetComplainForFinanceVM>>
            {
                Code = StatusCodes.Status200OK,
                Status = HttpStatusCode.OK.ToString(),
                Message = "Get all complain detail succeed",
                Data = results.ToList()
            });
        }
        catch
        {
            return NotFound(new ResponseVM<GetComplainForFinanceVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Data not found"
            });
        }
    }
}


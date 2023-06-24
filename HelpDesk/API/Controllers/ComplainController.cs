using API.Contracs;
using API.Models;
using API.ViewModel.Complain;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;
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
}

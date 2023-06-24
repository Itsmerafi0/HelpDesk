using API.Contracs;
using API.Models;
using API.ViewModel.Resolution;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResolutionController : BaseController<Resolution, ResolutionVM>
    {
        private readonly IResolutionRepository _resolutionRepository;
        private readonly IMapper<Resolution, ResolutionVM> _mapper;

        public ResolutionController(IResolutionRepository resolutionRepository, IMapper<Resolution, ResolutionVM> mapper) : base(resolutionRepository, mapper)
        {
            _resolutionRepository = resolutionRepository;
            _mapper = mapper;
        }
    }
}

using Client.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class ResolutionController : Controller
    {
        private readonly IResolutionRepository _resolutionRepository;

        public ResolutionController (IResolutionRepository resolutionRepository)
        {
            _resolutionRepository = resolutionRepository;
        }
      /*  public async Task<IActionResult> Index()
        {
            string 
        }*/
    }
}

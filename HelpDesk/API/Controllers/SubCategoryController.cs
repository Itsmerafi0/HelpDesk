using API.Contracs;
using API.Models;
using API.Repository;
using API.ViewModel.Complain;
using API.ViewModel.Response;
using API.ViewModel.SubCategory;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : BaseController<SubCategory, SubCategoryVM>
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper<SubCategory, SubCategoryVM> _subCategoryMapper;

        public SubCategoryController(ISubCategoryRepository subCategoryRepository, IMapper<SubCategory, SubCategoryVM> subCategoryMapper) : base(subCategoryRepository, subCategoryMapper)
        {
            _subCategoryRepository = subCategoryRepository;
            _subCategoryMapper = subCategoryMapper;
        }

        [HttpGet("Detail")]
        public IActionResult GetSubCategoryDetail()
        {
            try
            {

                var results = _subCategoryRepository.GetSubCategoryDetail();

                return Ok(new ResponseVM<List<SubCategoryDetailVM>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Get all sub category detail succeed",
                    Data = results.ToList()
                });
            }
            catch
            {
                return NotFound(new ResponseVM<SubCategoryDetailVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

        }
    }
}

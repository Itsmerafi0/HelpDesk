using API.Contracs;
using API.Models;
using API.ViewModel.SubCategory;
using Microsoft.AspNetCore.Mvc;

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
    }
}

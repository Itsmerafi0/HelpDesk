using API.Models;
using API.ViewModel.SubCategory;

namespace API.Contracs
{
    public interface ISubCategoryRepository : IGeneralRepository<SubCategory>
    {
        IEnumerable<SubCategoryDetailVM> GetSubCategoryDetail();
    }
}

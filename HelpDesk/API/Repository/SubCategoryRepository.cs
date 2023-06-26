using API.Contexts;
using API.Contracs;
using API.Models;
using API.ViewModel.SubCategory;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class SubCategoryRepository : GeneralRepository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(HelpDeskManagementDBContext context) : base(context)
        {

        }
        public IEnumerable<SubCategoryDetailVM> GetSubCategoryDetail()
        {
            var subCategories = GetAll();
            var categories = _dbContext.Categories.ToList();

            var subCategoryDetails = new List<SubCategoryDetailVM>();

            foreach (var subcategory in subCategories)
            {
                var category = _dbContext.Categories.FirstOrDefault(e => e.Guid == subcategory.Guid);
                if (category != null)
                {
                    var subCategoryDetail = new SubCategoryDetailVM
                    {
                        Guid = subcategory.Guid,
                        SubCategoryName = subcategory.Name,
                        CategoryName = category.CategoryName,
                        RiskLevel = subcategory.RiskLevel
                    };

                    subCategoryDetails.Add(subCategoryDetail);
                }
            }

            return subCategoryDetails;
        }
    }
}

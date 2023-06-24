using API.Contexts;
using API.Contracs;
using API.Models;

namespace API.Repository
{
    public class SubCategoryRepository : GeneralRepository<SubCategory>, ISubCategoryRepository
    {
        public SubCategoryRepository(HelpDeskManagementDBContext context) : base(context)
        {

        }
    }
}

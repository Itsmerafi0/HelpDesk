using API.Contexts;
using API.Contracs;
using API.Models;

namespace API.Repository
{
    public class CategoryRepository : GeneralRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

        }
    }
}

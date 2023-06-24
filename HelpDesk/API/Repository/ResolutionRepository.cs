using API.Contexts;
using API.Contracs;
using API.Models;

namespace API.Repository
{
    public class ResolutionRepository : GeneralRepository<Resolution>, IResolutionRepository
    {
        public ResolutionRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

        }
    }
}

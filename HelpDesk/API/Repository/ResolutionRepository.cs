using API.Contexts;
using API.Contracs;
using API.Models;
using API.Utility;

namespace API.Repository
{
    public class ResolutionRepository : GeneralRepository<Resolution>, IResolutionRepository
    {
        public ResolutionRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

        }
        public void UpdateStatus(Resolution resolution, StatusLevel newStatus)
        {
            resolution.Status = newStatus;

            _dbContext.Update(resolution);
            _dbContext.SaveChanges();
        }
    }
}

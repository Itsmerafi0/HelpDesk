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

            if (resolution.Status == StatusLevel.Done)
            {

                resolution.FinishedDate = DateTime.Now;
            }

            _dbContext.Update(resolution);
            _dbContext.SaveChanges();

        }

        public void UpdateResolvedBy(Resolution resolution, Guid resolvedBy)
        {
            resolution.ResolvedBy = resolvedBy;

            _dbContext.Update(resolution);
            _dbContext.SaveChanges();
        }

        public void UpdateResolutionNote(Resolution resolution, string note)
        {
            resolution.Notes = note;

            _dbContext.Update(resolution);
            _dbContext.SaveChanges();
        }
    }
}

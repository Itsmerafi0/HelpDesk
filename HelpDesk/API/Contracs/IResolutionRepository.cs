using API.Models;
using API.Utility;

namespace API.Contracs
{
    public interface IResolutionRepository : IGeneralRepository<Resolution>
    {
        void UpdateStatus(Resolution resolution, StatusLevel newStatus);
        void UpdateResolvedBy(Resolution resolution, Guid resolvedBy);
        void UpdateResolutionNote(Resolution resolution, string note);
    }
}


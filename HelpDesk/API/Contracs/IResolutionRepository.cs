using API.Models;
using API.Utility;

namespace API.Contracs
{
    public interface IResolutionRepository : IGeneralRepository<Resolution>
    {
        void UpdateStatus(Resolution resolution, StatusLevel newStatus);
    }
}

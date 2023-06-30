using Client.Models;
using Client.Utility;

namespace Client.Repositories.Interface
{
    public interface IResolutionRepository : IRepository<Resolution, Guid>
    {
        void UpdateStatus(Resolution resolution, StatusLevel newStatus);
    }
}

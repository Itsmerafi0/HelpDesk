using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface IComplainRepository : IRepository<Complain, Guid>
    {
        public Task<ResponseListVM<ComplainDetailVM>> GetAllComplainDetails();
        public Task<ResponseListVM<GetComplainForDevVM>> GetAllComplainDev();
        public Task<ResponseListVM<GetComplainForFinance>> GetAllComplainFinance();

    }
}

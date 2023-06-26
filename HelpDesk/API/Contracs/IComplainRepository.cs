using API.Models;
using API.ViewModel.Complain;

namespace API.Contracs
{
    public interface IComplainRepository : IGeneralRepository<Complain>
    {
        IEnumerable<ComplainDetailVM> GetAllComplainDetail();
        IEnumerable<GetComplainForDevVM> GetAllComplainDev();
        IEnumerable<GetComplainForFinanceVM> GetAllComplainFinance();

    }
}

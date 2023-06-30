using API.Models;
using API.ViewModel.Ticket;

namespace API.Contracs
{
    public interface ITicketRepository : IGeneralRepository<Ticket>
    {
        IEnumerable<TicketDetailVM> GetAllComplainDetail();
        IEnumerable<GetTicketForDevVM> GetAllComplainDev();
        IEnumerable<GetTicketForFinanceVM> GetAllComplainFinance();
        int CreateReso(TicketResoVM complainresoVM);

        string FindEmailByComplainGuid(Guid complainGuid);
    }
}

using API.Models;
using API.ViewModel.Ticket;

namespace API.Contracs
{
    public interface ITicketRepository : IGeneralRepository<Ticket>
    {
        bool CheckTicket(string value);


        IEnumerable<TicketDetailVM> GetAllComplainDetail();
        IEnumerable<GetTicketForDevVM> GetAllComplainDev();
        IEnumerable<GetTicketForFinanceVM> GetAllComplainFinance();
        int CreateTicketResolution(TicketResoVM complainresoVM);

        string FindEmailByComplainGuid(Guid complainGuid);
    }
}

using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface ITicketRepository : IRepository<Ticket, Guid>
    {
        public Task<ResponseListVM<TicketDetailVM>> GetAllTicketDetails();
        public Task<ResponseListVM<GetTicketForDevVM>> GetAllTicketDev();
        public Task<ResponseListVM<GetTicketForFinance>> GetAllTicketFinance();

        public Task<ResponseMessageVM> CreateTicket(TicketResoVM entity);

    }
}

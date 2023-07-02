using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface ITicketRepository : IRepository<Ticket, Guid>
    {
        public Task<ResponseListVM<TicketDetailVM>> GetAllTicketDetails();
        public Task<ResponseListVM<GetTicketForDevVM>> GetAllTicketDev(string jwtToken);
        public Task<ResponseListVM<GetTicketForFinance>> GetAllTicketFinance(string jwtToken);

        public Task<ResponseMessageVM> CreateTicket(TicketResoVM entity, string jwToken);

    }
}

using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories.Data
{
    public class TicketRepository : GeneralRepository<Ticket, Guid>, ITicketRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public TicketRepository(string request = "Ticket/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }

        public async Task<ResponseListVM<TicketDetailVM>> GetAllTicketDetails()
        {
            ResponseListVM<TicketDetailVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "TicketDetail").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<TicketDetailVM>>(apiResponse);
            }
            return entityVM;
        } 
        public async Task<ResponseListVM<GetTicketForDevVM>> GetAllTicketDev()
        {
            ResponseListVM<GetTicketForDevVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "TicketDetailDeveloper").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetTicketForDevVM>>(apiResponse);
            }
            return entityVM;
        }  
        public async Task<ResponseListVM<GetTicketForFinance>> GetAllTicketFinance()
        {
            ResponseListVM<GetTicketForFinance> entityVM = null;
            using (var response = httpClient.GetAsync(request + "TicketDetailFinance").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetTicketForFinance>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseMessageVM> CreateTicket(TicketResoVM entity)
        {
            ResponseMessageVM entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "CreateTicket", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }
    }
}

using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
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
        public async Task<ResponseListVM<GetTicketForDevVM>> GetAllTicketDev(string jwtToken)
        {
            ResponseListVM<GetTicketForDevVM> entityVM = null;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            using (var response = httpClient.GetAsync(request + "TicketDetailDeveloper").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetTicketForDevVM>>(apiResponse);
            }
            return entityVM;
        }  
        public async Task<ResponseListVM<GetTicketForFinance>> GetAllTicketFinance(string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ResponseListVM<GetTicketForFinance> entityVM = null;
            using (var response = httpClient.GetAsync(request + "TicketDetailFinance").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetTicketForFinance>>(apiResponse);
            }
            return entityVM;
        }

        public async Task<ResponseMessageVM> CreateTicket(TicketResoVM entity, string jwToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwToken);
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

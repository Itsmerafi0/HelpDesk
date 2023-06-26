using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;

namespace Client.Repositories.Data
{
    public class ComplainRepository : GeneralRepository<Complain, Guid>, IComplainRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public ComplainRepository(string request = "Complain/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }

        public async Task<ResponseListVM<ComplainDetailVM>> GetAllComplainDetails()
        {
            ResponseListVM<ComplainDetailVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "ComplainDetail").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<ComplainDetailVM>>(apiResponse);
            }
            return entityVM;
        } 
        public async Task<ResponseListVM<GetComplainForDevVM>> GetAllComplainDev()
        {
            ResponseListVM<GetComplainForDevVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "ComplainDetailDeveloper").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetComplainForDevVM>>(apiResponse);
            }
            return entityVM;
        }  
        public async Task<ResponseListVM<GetComplainForFinance>> GetAllComplainFinance()
        {
            ResponseListVM<GetComplainForFinance> entityVM = null;
            using (var response = httpClient.GetAsync(request + "ComplainDetailFinance").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<GetComplainForFinance>>(apiResponse);
            }
            return entityVM;
        }
    }
}

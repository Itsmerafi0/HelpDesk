using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Client.Repositories.Data
{
    public class EmployeeRepository : GeneralRepository<Employee, Guid>, IEmployeeRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public EmployeeRepository(string request = "Employee/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }
        public async Task<ResponseMessageVM> Registers(RegisterVM entity, string jwtToken)
        {
            ResponseMessageVM entityVM = null;

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "Register", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseMessageVM>(apiResponse);
            }
            return entityVM;
        }

/*        public async Task<ResponseViewModel<GetComplainForUserVM>> GetAllComplainUser(Guid guid)
        {
            ResponseViewModel<GetComplainForUserVM> entity = null;

            using (var response = await httpClient.GetAsync(request + guid))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entity = JsonConvert.DeserializeObject<ResponseViewModel<GetComplainForUserVM>>(apiResponse);
            }
            return entity;
        }
*/    }
}

using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace Client.Repositories.Data
{
    public class AccountRepository : GeneralRepository<Account, string>, IAccountRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public AccountRepository(string request = "account/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }

        public async Task<ResponseViewModel<string>> Logins(LoginVM entity)
        {
            ResponseViewModel<string> entityVM = null;
            StringContent content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
            using (var response = httpClient.PostAsync(request + "login", content).Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseViewModel<string>>(apiResponse);
            }
            return entityVM;
        }

       
    }
}

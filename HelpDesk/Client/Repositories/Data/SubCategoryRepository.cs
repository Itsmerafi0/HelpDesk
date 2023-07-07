using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace Client.Repositories.Data
{
    public class SubCategoryRepository : GeneralRepository<SubCategory, Guid>, ISubCategoryRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public SubCategoryRepository(string request = "subcategory/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };

        }

        public async Task<ResponseListVM<SubCategoryDetailVM>> GetAllSub(string jwtToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
            ResponseListVM<SubCategoryDetailVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "detail").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<SubCategoryDetailVM>>(apiResponse);
            }
            return entityVM;
        }

    }
}

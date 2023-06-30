using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Newtonsoft.Json;

namespace Client.Repositories.Data
{
    public class SubCategoryRepository : GeneralRepository<SubCategory, Guid>, ISubCategoryRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public SubCategoryRepository(string request = "SubCategory/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };

        }

        public async Task<ResponseListVM<SubCategoryDetailVM>> GetAllSub()
        {
            ResponseListVM<SubCategoryDetailVM> entityVM = null;
            using (var response = httpClient.GetAsync(request + "Detail").Result)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                entityVM = JsonConvert.DeserializeObject<ResponseListVM<SubCategoryDetailVM>>(apiResponse);
            }
            return entityVM;
        }

    }
}

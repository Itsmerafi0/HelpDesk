using Client.Models;
using Client.Repositories.Interface;

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
    }
}

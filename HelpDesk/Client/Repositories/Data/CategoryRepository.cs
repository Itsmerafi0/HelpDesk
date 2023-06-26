using Client.Models;
using Client.Repositories.Interface;

namespace Client.Repositories.Data
{
    public class CategoryRepository : GeneralRepository<Category, Guid>, ICategoryRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public CategoryRepository(string request = "Category/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }
    }
}

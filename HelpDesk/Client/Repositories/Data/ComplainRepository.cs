using Client.Models;
using Client.Repositories.Interface;

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
                BaseAddress = new Uri("https://localhost:7281/api/")
            };
        }
    }
}

using Client.Models;
using Client.Repositories.Interface;
using System.Runtime.CompilerServices;

namespace Client.Repositories.Data
{
    public class ResolutionRepository : GeneralRepository<Resolution, Guid>, IResolutionRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public ResolutionRepository(string request = "resolution/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }
    }
}

using Client.Models;
using Client.Repositories.Interface;

namespace Client.Repositories.Data
{
    public class RoleRepository : GeneralRepository<Role, Guid>, IRoleRepository
    {
        private readonly HttpClient httpClient;
        private readonly string request;

        public RoleRepository(string request = "Role/") : base(request)
        {
            this.request = request;
            httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7024/api/")
            };
        }
    }
}

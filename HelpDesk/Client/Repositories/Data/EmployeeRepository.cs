using Client.Models;
using Client.Repositories.Interface;

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
                BaseAddress = new Uri("https://localhost:7281/api/")
            };
        }
    }
}

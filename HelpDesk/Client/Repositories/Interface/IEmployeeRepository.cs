using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface IEmployeeRepository : IRepository<Employee, Guid>
    {
        public Task<ResponseMessageVM> Registers(RegisterVM entity, string jwtToken);

       /* GetComplainForUserVM GetAllComplainUser(Guid guid);*/
    }
}

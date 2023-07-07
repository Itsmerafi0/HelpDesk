using API.Models;
using API.ViewModel.Account;

namespace API.Contracs
{
    public interface IAccountRepository : IGeneralRepository<Account>
    {
      

        LoginVM Login(LoginVM loginVM);

        IEnumerable<string> GetRoles(Guid guid);

    }
}

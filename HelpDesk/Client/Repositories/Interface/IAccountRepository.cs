using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface IAccountRepository : IRepository<Account, string>
    {
        public Task<ResponseViewModel<string>> Logins(LoginVM entity);
       
    }
}

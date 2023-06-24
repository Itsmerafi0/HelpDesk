using API.Contexts;
using API.Contracs;
using API.Models;

namespace API.Repository
{
    public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
    {
        public AccountRoleRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {
        }
    }
}

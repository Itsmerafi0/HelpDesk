using API.Contexts;
using API.Contracs;
using API.Models;

namespace API.Repository
{
    public class RoleRepository : GeneralRepository<Role>, IRoleRepository
    {
        public RoleRepository(HelpDeskManagementDBContext dbContext) : base(dbContext) { }
    }
}

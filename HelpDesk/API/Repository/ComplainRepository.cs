using API.Contexts;
using API.Models;


namespace API.Repository
{
    public class ComplainRepository : GeneralRepository<Complain>, Contracs.IComplainRepository
    {
        public ComplainRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {
        }
    }
}

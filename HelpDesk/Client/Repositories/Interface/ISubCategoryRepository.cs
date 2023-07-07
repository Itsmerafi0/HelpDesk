using Client.Models;
using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface ISubCategoryRepository : IRepository<SubCategory, Guid>
    {
        public Task<ResponseListVM<SubCategoryDetailVM>> GetAllSub(string jwtToken);

    }
}

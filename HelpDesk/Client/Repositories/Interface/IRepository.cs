using Client.ViewModels;

namespace Client.Repositories.Interface
{
    public interface IRepository<T, X>
        where T : class
    {
        Task<ResponseListVM<T>> Gets();
        Task<ResponseViewModel<T>> Gets(X guid);
        Task<ResponseMessageVM> Posts(T entity);
        Task<ResponseMessageVM> Puts(T entity);
        Task<ResponseMessageVM> Deletes(X guid);
    }
}

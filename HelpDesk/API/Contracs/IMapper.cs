namespace API.Contracs
{

        public interface IMapper<TModel, TViewModel>
        {
            TViewModel Map(TModel model);
            TModel Map(TViewModel viewmodel);
           /* object Map(AccountEmployeeVM accountEmployeeVM);*/
        }
}

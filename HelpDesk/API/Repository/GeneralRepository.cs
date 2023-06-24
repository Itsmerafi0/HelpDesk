using API.Contexts;
using API.Contracs;

namespace API.Repository
{
    public class GeneralRepository<Tentity> : IGeneralRepository<Tentity> where Tentity : class
    {
        protected readonly HelpDeskManagementDBContext _dbContext;
        public GeneralRepository(HelpDeskManagementDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Tentity? Create(Tentity entity)
        {
            try
            {
                typeof(Tentity).GetProperty("CreatedDate")!.SetValue(entity, DateTime.Now);
                typeof(Tentity).GetProperty("ModifiedDate")!.SetValue(entity, DateTime.Now);
                _dbContext.Set<Tentity>().Add(entity);
                _dbContext.SaveChanges();
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(Guid guid)
        {
            try
            {
                var entity = GetByGuid(guid);
                if (entity == null)
                {
                    return false;
                }

                _dbContext.Set<Tentity>().Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Tentity> GetAll()
        {
            return _dbContext.Set<Tentity>().ToList();
        }

        public Tentity? GetByGuid(Guid guid)
        {
            var entity = _dbContext.Set<Tentity>().Find(guid);
            _dbContext.ChangeTracker.Clear();
            return entity;
        }

        public bool Update(Tentity entity)
        {
            try
            {
                var guid = (Guid)typeof(Tentity).GetProperty("Guid")!
                                                    .GetValue(entity)!;
                var oldEntity = GetByGuid(guid);
                if (oldEntity == null)
                {
                    return false;
                }
                var getCreatedDate = typeof(Tentity).GetProperty("CreatedDate")!
                                                    .GetValue(oldEntity)!;

                typeof(Tentity).GetProperty("CreatedDate")!
                               .SetValue(entity, getCreatedDate);
                typeof(Tentity).GetProperty("ModifiedDate")!
                               .SetValue(entity, DateTime.Now);

                _dbContext.Set<Tentity>().Update(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }


}

namespace API.Contracs
{
    public interface IGeneralRepository<Tentity>
    {
        Tentity? Create(Tentity item);
        bool Update(Tentity item);
        bool Delete(Guid guid);
        IEnumerable<Tentity> GetAll();
        Tentity? GetByGuid(Guid guid);
    }
}

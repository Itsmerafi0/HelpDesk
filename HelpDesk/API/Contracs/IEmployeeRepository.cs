using API.Models;

namespace API.Contracs
{
    public interface IEmployeeRepository : IGeneralRepository<Employee>
    {
        bool CheckEmailAndPhoneAndNIK(string value);

        Employee GetEmail(string email);

        Guid? FindGuidByEmail(string email);
    }
}

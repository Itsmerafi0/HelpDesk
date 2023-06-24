using API.Contexts;
using API.Contracs;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HelpDeskManagementDBContext context) : base(context) { }
        public bool CheckEmailAndPhoneAndNIK(string value)
        {
            return _dbContext.Employees.Any(e => e.Email == value ||
                                            e.PhoneNumber == value ||
                                            e.Nik == value);
        }

        public Guid? FindGuidByEmail(string email)
        {
            try
            {
                var employee = _dbContext.Employees.FirstOrDefault(e => e.Email == email);
                if (employee == null)
                {
                    return null;
                }
                return employee.Guid;
            }
            catch
            {
                return null;
            }

        }

        public Employee GetEmail(string email)
        {
            var employee = _dbContext.Set<Employee>().FirstOrDefault(e => e.Email == email);

            return employee;
        }
    }
}

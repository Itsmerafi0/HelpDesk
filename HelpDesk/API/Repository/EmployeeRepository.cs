using API.Contexts;
using API.Contracs;
using API.Models;
using API.Utility;
using API.ViewModel.Employees;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HelpDeskManagementDBContext context, IAccountRepository accountRepository) : base(context) 
        {
        _accountRepository = accountRepository;
        }

        private readonly IAccountRepository _accountRepository;

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

        public int Register(RegisterVM registerVM)
        {
            try
            {
                var employee = new Employee
                {
                    Nik = GenerateNIK(),
                    FirstName = registerVM.FirstName,
                    LastName = registerVM.LastName,
                    BirthDate = registerVM.BirthDate,
                    Gender = registerVM.Gender,
                    HiringDate = registerVM.HiringDate,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber
                };
                var result = Create(employee);

                var account = new Account
                {
                    Guid = employee.Guid,
                    Password = Hashing.HashPassword(registerVM.Password),
                    IsDeleted = false,
                    IsUsed = false,
                    OTP = 0
                };

                _accountRepository.Create(account);

                var accountrole = new AccountRole
                {
                    RoleGuid = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
                    AccountGuid = employee.Guid
                };
                _dbContext.AccountRoles.Add(accountrole);
                _dbContext.SaveChanges();

                return 3;
            }
            catch
            {
                return 0;
            }

        }
        private string GenerateNIK()
        {
            var lastNik = GetAll().OrderByDescending(e => int.Parse(e.Nik)).FirstOrDefault();

            if (lastNik != null)
            {
                int lastNikNumber;
                if (int.TryParse(lastNik.Nik, out lastNikNumber))
                {
                    return (lastNikNumber + 1).ToString();
                }
            }

            return "100000";
        }

        public GetComplainForUserVM GetAllComplainUser(Guid guid)
        {
            var employee = GetByGuid(guid);
            var complain = _dbContext.Complains.FirstOrDefault(e => e.EmployeeGuid == guid);
            var subCategory = _dbContext.SubCategories.FirstOrDefault(sb => sb.Guid == complain.SubCategoryGuid);
            var category = _dbContext.Categories.FirstOrDefault(c => c.Guid == subCategory.CategoryGuid);
            /*     var resolution = _dbContext.Resolutions.FirstOrDefault(r => r.Guid ==complain.Guid);*/

            var data = new GetComplainForUserVM
            {
                Guid = employee.Guid,
                Description = complain.Description,
                Attachment = complain.Attachment,
                CategoryName = category.CategoryName,
                SubCategoryName = subCategory.Name,
                /*StatusLevel = resolution.Status*/
            };
            return data;
        }
    }
}

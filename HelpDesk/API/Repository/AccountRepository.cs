using API.Models;
using API.Utility;
using API.ViewModel.Account;
using API.Contexts;
using API.Contracs;
using API.Models;
using API.ViewModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace API.Repository
{
    public class AccountRepository : GeneralRepository<Account>, IAccountRepository
    {
        public AccountRepository(HelpDeskManagementDBContext dbContext,
            IEmployeeRepository employeeRepository) : base(dbContext)
        {
            _employeerepository = employeeRepository;
        }

        private readonly IEmployeeRepository _employeerepository;

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
                var result = _employeerepository.Create(employee);

                var account = new Account
                {
                    Guid = employee.Guid,
                    Password = Hashing.HashPassword(registerVM.Password),
                    IsDeleted = false,
                    IsUsed = false,
                    OTP = 0
                };

                Create(account);

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
            var lastNik = _employeerepository.GetAll().OrderByDescending(e => int.Parse(e.Nik)).FirstOrDefault();

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

        public LoginVM Login(LoginVM loginVM)
        {


            var account = GetAll();
            var employee = _dbContext.Employees.ToList();


            var query = from emp in employee
                        join acc in account
                        on emp.Guid equals acc.Guid
                        where emp.Email == loginVM.Email
                        select new LoginVM
                        {
                            Email = emp.Email,
                            Password = acc.Password
                        };
            var data = query.FirstOrDefault();

            if (data != null && Hashing.ValidatePassword(loginVM.Password, data.Password))
            {
                loginVM.Password = data.Password;
            }
            return data;
        }


        public IEnumerable<string> GetRoles(Guid Guid)
        {
            var getAccount = GetByGuid(Guid);
            if (getAccount == null) return Enumerable.Empty<string>();
            var getAccountRoles = from accountRoles in _dbContext.AccountRoles
                                  join roles in _dbContext.Roles on accountRoles.RoleGuid equals roles.Guid
                                  where accountRoles.AccountGuid == Guid
                                  select roles.Name;

            return getAccountRoles.ToList();

        }

    }
}

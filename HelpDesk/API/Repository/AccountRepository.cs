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
        public AccountRepository(HelpDeskManagementDBContext dbContext) : base(dbContext)
        {

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

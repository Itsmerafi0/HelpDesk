using API.Contracs;
using API.Models;
using API.ViewModel.AccountRole;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AccountRoleController : BaseController<AccountRole, AccountRoleVM>
{
    private readonly IAccountRoleRepository _accountroleRepository;
    private readonly IMapper<AccountRole, AccountRoleVM> _mapper;
    public AccountRoleController(IAccountRoleRepository accountroleRepository,
        IMapper<AccountRole, AccountRoleVM> mapper) : base(accountroleRepository, mapper)
    {
        _accountroleRepository = accountroleRepository;
        _mapper = mapper;
    }
}

using API.Contracs;
using API.Models;
using API.Utility;
using API.ViewModel.Account;
using API.ViewModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class accountController : BaseController<Account, AccountVM>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper<Account, AccountVM> _mapper;
    private readonly ITokenService _tokenService;
    public accountController(IAccountRepository accountRepository,
                             IMapper<Account, AccountVM> mapper,
                             IEmployeeRepository employeeRepository,
                             ITokenService tokenRepository) : base(accountRepository, mapper)
    {
        _mapper = mapper;
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _tokenService = tokenRepository;
    }
   

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login(LoginVM loginVM)
    {
        var account = _accountRepository.Login(loginVM);
        var employee = _employeeRepository.GetEmail(loginVM.Email);

        if (account == null)
        {
            return NotFound(new ResponseVM<LoginVM>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Account Not Found"
            });
        }

        if (account.Password != loginVM.Password)
        {
            return BadRequest(new ResponseVM<LoginVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Password Invalid"
            });
        }

        var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, employee.Guid.ToString()),
                new(ClaimTypes.Name, $"{employee.FirstName}{employee.LastName}"),
                new(ClaimTypes.Email, employee.Email),
            };

        var roles = _accountRepository.GetRoles(employee.Guid);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = _tokenService.GenerateToken(claims);


        return Ok(new ResponseVM<string>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Login Success",
            Data = token
        });

    }

}

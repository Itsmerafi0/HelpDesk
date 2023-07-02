using API.Contracs;
using API.Models;
using API.ViewModel.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]

public class RoleController : BaseController<Role, RoleVM>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper<Role, RoleVM> _mapper;
    public RoleController(IRoleRepository roleRepository,
        IMapper<Role, RoleVM> mapper) : base(roleRepository, mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
}

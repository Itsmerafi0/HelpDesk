﻿using API.Contracs;
using API.Models;
using API.ViewModel.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]

public class roleController : BaseController<Role, RoleVM>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper<Role, RoleVM> _mapper;
    public roleController(IRoleRepository roleRepository,
        IMapper<Role, RoleVM> mapper) : base(roleRepository, mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }
}

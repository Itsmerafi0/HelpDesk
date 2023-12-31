﻿using API.Contracs;
using API.Models;
using API.Repository;
using API.ViewModel.Account;
using API.ViewModel.Employees;
using API.ViewModel.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]


    public class employeeController : BaseController<Employee, EmployeeVM>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper<Employee, EmployeeVM> _mapper;

        public employeeController(IEmployeeRepository employeeRepository, IMapper<Employee, EmployeeVM> mapper) : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public IActionResult Register(RegisterVM registerVM)
        {
            var result = _employeeRepository.Register(registerVM);
            switch (result)
            {
                case 0:
                    return BadRequest(new ResponseVM<AccountVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Register Failed"
                    });

                case 1:
                    return BadRequest(new ResponseVM<AccountVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Email Already Exists"
                    });

                case 2:
                    return BadRequest(new ResponseVM<AccountVM>
                    {
                        Code = StatusCodes.Status400BadRequest,
                        Status = HttpStatusCode.BadRequest.ToString(),
                        Message = "Phone Already Exists"
                    });
                case 3:
                    return Ok(new ResponseVM<AccountVM>
                    {
                        Code = StatusCodes.Status200OK,
                        Status = HttpStatusCode.OK.ToString(),
                        Message = "Registration Success"
                    });
            }
            return Ok(new ResponseVM<AccountVM>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Registration Success"
            });
        }
        [HttpGet("DeveloperAndFinanceDetails")]
        public IActionResult GetDeveloperAndFinanceDetails()
        {
            try
            {
                var results = _employeeRepository.GetDevAndFinanceDetails();

                return Ok(new ResponseVM<List<DevAndFinanceDetailsVM>>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Get all developer and finance details succeed",
                    Data = results.ToList()
                });
            }
            catch
            {
                return NotFound(new ResponseVM<DevAndFinanceDetailsVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }
        }

    }
}

/*        [HttpGet("ComplainDetailUser")]

        public IActionResult GetAllComplainUser(Guid guid)
        {
            try
            {
                var results = _employeeRepository.GetAllComplainUser(guid);

                return Ok(new ResponseVM<GetComplainForUserVM>
                {
                    Code = StatusCodes.Status200OK,
                    Status = HttpStatusCode.OK.ToString(),
                    Message = "Get all complain detail succeed",
                    Data = results
                });
            }
            catch
            {
                return NotFound(new ResponseVM<GetComplainForUserVM>
                {
                    Code = StatusCodes.Status404NotFound,
                    Status = HttpStatusCode.NotFound.ToString(),
                    Message = "Data not found"
                });
            }

        }
*/

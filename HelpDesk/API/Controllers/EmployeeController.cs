using API.Contracs;
using API.Models;
using API.ViewModel.Employees;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : BaseController<Employee, EmployeeVM>
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper<Employee, EmployeeVM> _mapper;

        public EmployeeController(IEmployeeRepository employeeRepository, IMapper<Employee, EmployeeVM> mapper) : base(employeeRepository, mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }
    }
}

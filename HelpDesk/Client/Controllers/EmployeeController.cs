using Client.Models;
using Client.Repositories.Interface;
using Client.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository repository;

        public EmployeeController(IEmployeeRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await repository.Gets();
            var employees = new List<Employee>();

            if (result.Data != null)
            {
                employees = result.Data.Select(e => new Employee
                {
                    Guid = e.Guid,
                    Nik = e.Nik,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    BirthDate = e.BirthDate,
                    Gender = e.Gender,
                    HiringDate = e.HiringDate,
                    Email = e.Email,
                    PhoneNumber = e.PhoneNumber,
                }).ToList();
            }
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Employee employee)
        {
           
                var result = await repository.Puts(employee);
                if (result.Code == 200)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (result.Code == 409)
                {
                    ModelState.AddModelError(string.Empty, result.Message);
                    return View();
                }
            
            return View();
        }

        public async Task<IActionResult> Edit(Guid Guid)
        {
            var result = await repository.Gets(Guid);
            var employee = new Employee();
            if (result.Data?.Guid is null)
            {
                return View(employee);
            }
            else
            {
                employee.Guid = result.Data.Guid;
                employee.Nik = result.Data.Nik;
                employee.FirstName = result.Data.FirstName;
                employee.LastName = result.Data.LastName;
                employee.BirthDate = result.Data.BirthDate;
                employee.Gender = result.Data.Gender;
                employee.HiringDate = result.Data.HiringDate;
                employee.Email = result.Data.Email;
                employee.PhoneNumber = result.Data.PhoneNumber;
                employee.CreatedDate = result.Data.CreatedDate;
                employee.ModifiedDate = DateTime.Now;
            }

            return View(employee);
        }
        public async Task<IActionResult> Deletes(Guid guid)
        {
            var result = await repository.Gets(guid);
            var employees = new Employee();
            if (result.Data?.Guid is null)
            {
                return View(employees);
            }
            else
            {
                employees.Guid = result.Data.Guid;
                employees.Nik = result.Data.Nik;
                employees.FirstName = result.Data.FirstName;
                employees.LastName = result.Data.LastName;
                employees.BirthDate = result.Data.BirthDate;
                employees.Gender = result.Data.Gender;
                employees.HiringDate = result.Data.HiringDate;
                employees.Email = result.Data.Email;
                employees.PhoneNumber = result.Data.PhoneNumber;

            }
            return View(employees);
        }

        [HttpPost]
        //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid guid)
        {
            var result = await repository.Deletes(guid);
            if (result.Code == 200)
            {
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Registers(RegisterVM registerVM)
        {

            var result = await repository.Registers(registerVM);
            if (result is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
                return View();
            }
            else if (result.Code == 200)
            {
                TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
                return RedirectToAction("Index", "Employee");
            }
            return View();
        }
    }
}

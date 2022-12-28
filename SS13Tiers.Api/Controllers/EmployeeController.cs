using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SS13Tiers.Api.Data;
using SS13Tiers.Api.Models;

namespace SS13Tiers.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/Employee
        [HttpGet]
        public async Task<IEnumerable<Employee>> Get()
            => await _context.Employee.ToListAsync();

        // GET: api/Employee/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Employee> Get(Guid id)
            => await _context.Employee.FindAsync(id);

        // POST: api/Employee
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (await IsExist(employee.EmployeeName, employee.DateOfBirth))
                    return BadRequest("Employee existing");
                employee.EmployeeId = Guid.NewGuid();
                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();
                return Created("", employee);
            }
            return BadRequest(ModelState);
        }
        private async Task<bool> IsExist(string name, DateTime dob)
            => await _context.Employee.AnyAsync(x => x.EmployeeName.Equals(name) && x.DateOfBirth.Equals(dob));

        // PUT: api/Employee/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Employee employee)
        {
            if(id == employee.EmployeeId || ModelState.IsValid)
            {
                // Find record
                var emp = await _context.Employee.FindAsync(id);
                if (emp is null) return BadRequest("Invalid Employee Id");
                _context.Employee.Attach(emp);
                emp.Address = employee.Address;
                emp.DateOfBirth = employee.DateOfBirth;
                emp.DepartmentId = employee.DepartmentId;
                emp.EmployeeName = employee.EmployeeName;
                emp.Gender = employee.Gender;
                emp.PhoneNumber = employee.PhoneNumber;
                emp.PlaceOfBirth = employee.PlaceOfBirth;
                emp.PositionId = employee.PositionId;
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee is null) return BadRequest("Invalid employee id");
            _context.Employee.Remove(employee);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

using APIEmployeeDepartmentProject.Data;
using APIEmployeeDepartmentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace APIEmployeeDepartmentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public EmployeesController(AppDbContext db)
        {
            _db = db;
        }


        // ENDPOINTS

        // [GET] /api/employees - Return all employees
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAll()
        {
            // Also shows the department the employee belongs to
            var employees = await _db.Employees.Include(e => e.Department)
                .AsNoTracking().ToListAsync();  // AsNoTracking for read only
            return employees;
        }

        // [GET] /api/employees/id - get employee by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _db.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.id == id);

            if (employee == null) {
                return NotFound();  // Return 404 if not employee has the specific id
             }

            return employee;
        }

        // [POST] - /api/employees - Create an employee. A department must exists - DepartmentID
        [HttpPost]
        public async Task<ActionResult<Employee>> Create(Employee employee)
        {
            // Ensure Department exists

            var depExists = await _db.Departments.AnyAsync(d => d.id == employee.DepartmentId);

            // 400 Bad Request incase the department does not exist
            if (!depExists)
            {
                return BadRequest($"Department ID {employee.DepartmentId} do not exist");
            }

            _db.Employees.Add(employee); // INSERT new employee
            await _db.SaveChangesAsync(); // EXECUTE INSERT


            // 201 Created //  URL to get employee // + body 
            return CreatedAtAction(nameof(GetById), new { id = employee.id }, employee);
        }

        // [PUT] /api/employee/id - updates employee by id 
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, Employee updated)
        {
            // if invalid body request or missing
            if (updated == null) {
                return BadRequest("Employee body request requierd");
            }

            // find existing employee
            var exists = await _db.Employees.FindAsync(id);

            // 404 not found if id do not exists
            if (exists == null) {
                return NotFound();
            }

            // When updating to new department make sure new department exists

            if(exists.DepartmentId != updated.DepartmentId)
            {
                bool depExists = await _db.Departments.AnyAsync(d => d.id == updated.DepartmentId);
                if (!depExists)
                {
                    return BadRequest($"Department ID {updated.DepartmentId} does not exist");
                }
            }

            // Update body from updated objet to existing entity
            exists.FirstName = updated.FirstName;
            exists.LastName = updated.LastName;
            exists.EmailAddress = updated.EmailAddress;
            exists.Salary = updated.Salary;
            exists.DepartmentId = updated.DepartmentId;

            // Save changes to database
            await _db.SaveChangesAsync();

            // Return 204 success
            return NoContent();
               
        }

        // [DELETE] /api/employee/id - delete an employee by id
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {   
            // find employee by id
            var exists = await _db.Employees.FindAsync(id);

            // if id was not found, 404 not found
            if (exists == null)
            {
                return NotFound();
            }

            // deletion
            _db.Employees.Remove(exists);

            // EXECUTE deletion in database
            await _db.SaveChangesAsync();

            return NoContent(); //  Return 204 success 

        }

    }
}

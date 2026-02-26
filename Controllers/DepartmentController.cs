using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using APIEmployeeDepartmentProject.Data;
using APIEmployeeDepartmentProject.Models;

namespace APIEmployeeDepartmentProject.Controllers
{
    // Enables API specific responses on validation errors
    [ApiController]

    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AppDbContext _db;

        // Inject AppDbContext by ASP.NET Core 
        public DepartmentsController(AppDbContext db) {
            _db = db;
        }


        // ENDPOINTS

        // [GET] /api/departments - returns a list with all departments
        [HttpGet]
        public async Task<ActionResult<List<Department>>> GetAll() {

            // read only query for better performance using as AsNoTracking
            var departments = await _db.Departments.AsNoTracking().ToListAsync();

            return departments;
        }

        // [GET] /api/departments/id - returns one department by id

        [HttpGet("id")]
        public async Task<ActionResult<Department>> GetById(int id) {

            var dep = await _db.Departments.FindAsync(id);

            // if no department exists with specific id, error 404
            if (dep == null) {
                return NotFound();
            }
            return dep;
        }

        //[POST] /api/departments - create new department
        [HttpPost]
        public async Task<ActionResult<Department>> Create(Department department){

            // if name or office location are missing , ASP.NET returns 400 BadRequest
            
            _db.Departments.Add(department); // INSERT
            await _db.SaveChangesAsync();    // EXECUTE INSERT in SQL Server

            // 201 status code - GET  by id 
            return CreatedAtAction(nameof(GetById), new { id = department.id },department);
        }

        // [PUT] /api/department/id - Updates an existing department
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, Department updated){
            // find existing department
            var exists = await _db.Departments.FindAsync(id);
            if (exists == null) {
                return NotFound();  // 404 Not found
            }

            // provide the fields allowed to update
            exists.Name = updated.Name;
            exists.OfficeLocation = updated.OfficeLocation;

            await _db.SaveChangesAsync(); // UPDATE
            return NoContent();           // 204 status code for succesfully communcation with server
        }

        //[DELETE] /api/departments/id - delete a department 
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id){

            var exists = await _db.Departments.FindAsync(id);

            if (exists == null){
                return NotFound();  // 404 not found
            }

            _db.Departments.Remove(exists);
            await _db.SaveChangesAsync(); // EXECUTE DELETE

            return NoContent(); // 204 success
        }
    }
}

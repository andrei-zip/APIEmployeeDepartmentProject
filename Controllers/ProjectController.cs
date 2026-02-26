using Microsoft.AspNetCore.Mvc;
using APIEmployeeDepartmentProject.Data;
using APIEmployeeDepartmentProject.Models;
using APIEmployeeDepartmentProject.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;


namespace APIEmployeeDepartmentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IRandomStringGenerator _randomStringGenerator;

        // Injection of AppDbContext to SQL Server and IRandomStringGenerator to call external API

        public ProjectController(AppDbContext db,IRandomStringGenerator randomStringGenerator)
        {
            _db = db; 
            _randomStringGenerator = randomStringGenerator;
        }
        // [GET] - /api/projects - returns a list of all projects
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetAll()
        {
            var projects = await _db.Projects.ToListAsync();
            return projects;
        }

        // [GET] -  /api/projects/id -  returns a project by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> GetById(int id)
        {
            var project = await _db.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound(); // 
            }
            return project;
        }


        //[POST] - /api/projects - creates project with unique projectcode using random code generator + project id
        [HttpPost]
        public async Task<ActionResult<Project>> Create(Project project,CancellationToken ct)
        {

            // check for nulls
            if (project == null)
            {
                return BadRequest("Project body required");
            }

            // generate ProjectCode
            project.ProjectCode = "TEMPORARY-" + Guid.NewGuid().ToString("N"); // UNIQUE placeholder

            // Database transaction - if fails - rollback so the project wont be saved without a final code
            await using var transaction = await _db.Database.BeginTransactionAsync(ct);

            try
            {
                // add placeholder code to project
                _db.Projects.Add(project);

                // sql server generates id
                await _db.SaveChangesAsync(ct);

                // call external api to generate the random code
                var randomCode = await _randomStringGenerator.GenerateAsync(ct);

                // Append the project id with random code
                var finalCode = $"{randomCode}-{project.id}";

                // set final code
                project.ProjectCode = finalCode;

                // UPDATE the db
                await _db.SaveChangesAsync(ct);

                // end transaction
                await transaction.CommitAsync(ct);

                // return 201 succesful creating to get by id
                return CreatedAtAction(nameof(GetById), new { id = project.id }, project);
            }
            catch (Exception ex) {
            
                // if anything failes rollback transaction so project wont exist without a valid ProjectCode
                await transaction.RollbackAsync(ct);

                // return the message and status code
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }

        //[PUT] - /apo/projects/id - update projects by id only allowing name and budget NOT projectcode
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,Project updated)
        {
            if (updated == null)
            {
                return BadRequest("Project body required");
            }

            var exist = await _db.Projects.FindAsync(id);
            if (exist == null)
            {
                return NotFound();
            }

            exist.name = updated.name;
            exist.Budget = updated.Budget;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        //[DELETE] - /api/projects/id - delete project by unique id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var exist = await _db.Projects.FindAsync(id);
            if (exist == null)
            {
                return NotFound();
            }

            _db.Projects.Remove(exist);
            await _db.SaveChangesAsync();

            return NoContent();

        }
    }
}

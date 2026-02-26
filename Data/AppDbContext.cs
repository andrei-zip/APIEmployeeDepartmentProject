using APIEmployeeDepartmentProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEmployeeDepartmentProject.Data
{
    // EF Core main class, ensure communication and access to database.
    public class AppDbContext : DbContext
    {
        // ASP.NET Core dependency injection provides the options such as SQL Server,connection strings, etc.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // EF Core utilizes EF Core to make chances and run queries in the dabase
        public DbSet<Department> Departments => Set<Department>();
    }
}

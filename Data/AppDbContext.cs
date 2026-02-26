using APIEmployeeDepartmentProject.Models;
using Microsoft.EntityFrameworkCore;

namespace APIEmployeeDepartmentProject.Data
{
    // EF Core main class, ensure communication and access to database.
    public class AppDbContext : DbContext
    {
        // Constructor
        // ASP.NET Core dependency injection provides the options such as SQL Server,connection strings, etc.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tables
        // EF Core utilizes EF Core to make chances and run queries in the dabase
        public DbSet<Department> Departments => Set<Department>();

        public DbSet<Employee> Employees => Set<Employee>();


        // Configs for relationships keys , indexes and entites
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Make email unique between employees
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmailAddress).IsUnique();

            // Configure decimal precision to avoid truncation - no alloweing to lose any cents
            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(18, 2);

        }

            
    }
}

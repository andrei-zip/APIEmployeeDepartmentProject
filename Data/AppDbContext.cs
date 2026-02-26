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

        public DbSet<Project> Projects => Set<Project>();

        //join table for employees and projects
        public DbSet<EmployeeProject> EmployeesProjects => Set<EmployeeProject>();

        // Configs for relationships keys , indexes and entites
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Employee Rules //
            // Make email unique between employees
            modelBuilder.Entity<Employee>().HasIndex(e => e.EmailAddress).IsUnique();

            // Configure decimal precision to avoid truncation - no alloweing to lose any cents
            modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(18, 2);

            // Project rules //
            // Project code must be unique
            modelBuilder.Entity<Project>().HasIndex(p=>p.ProjectCode).IsUnique();

            // Project code is required
            modelBuilder.Entity<Project>().Property(p=> p.ProjectCode).IsRequired().HasMaxLength(50);

            //Budget precision to avoid truncation
            modelBuilder.Entity<Project>().Property(p=>p.Budget).HasPrecision(18, 2);

            // EmployeeProject rules //
            // Makes sure an employee gets assigned only once per project
            modelBuilder.Entity<EmployeeProject>().HasKey(ep => new { ep.EmployeeId, ep.ProjectId });

            // Relationship - Many Projects per Employee
            modelBuilder.Entity<EmployeeProject>().HasOne(ep => ep.Employee)
                .WithMany(e => e.EmployeeProjects)
                .HasForeignKey(ep => ep.EmployeeId);

            // Relationship - Many Employees per Project
            modelBuilder.Entity<EmployeeProject>().HasOne(ep => ep.Project)
                .WithMany(p => p.EmployeeProjects)
                .HasForeignKey(ep => ep.ProjectId);

        }
    }
}

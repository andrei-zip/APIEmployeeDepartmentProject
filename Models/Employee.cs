using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIEmployeeDepartmentProject.Models
{
    // A class representing an Employee in the company
    // This will be mapped to the SQL table "Employees"
    public class Employee
    {
        // EF Core will automailically treat the "id" primary key in database
        public int id { get; set; }

        [Required, MaxLength(100)]

        // First name
        public string FirstName { get; set; }

        [Required, MaxLength(100)]

        // Last name
        public string LastName { get; set; }

        // Email address format
        [Required, EmailAddress, MaxLength(120)]
        public string EmailAddress { get; set; }

        // Dont allow negative salary
        [Range(0, double.MaxValue)]
        public decimal Salary { get; set; }

        // An employee belongs to one department - Foreign key *
        public int DepartmentId { get; set; }

        // allows acces to the department 
        public Department Department { get; set; }

        // icollection for projects
    }
}

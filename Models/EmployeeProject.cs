using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace APIEmployeeDepartmentProject.Models
{
    // A class representing a join table between Employees and Projects ( many to many)
    // Representing the the one to one relationship metween one employee and one project with a role 
    public class EmployeeProject
    {

        // The foreign key of Employee
        public int EmployeeId { get; set; }

        // Allows acces to EmployeePorject - Employee
        public Employee Employee { get; set; }

        //The foreign key of Project
        public int ProjectId { get; set; }

        // Allows access to EmployeeProject - Project
        public Project Project { get; set; }

        // Role of the employee in the project 
        [Required, MaxLength(100)]
        public string Role { get; set; }
    }
}

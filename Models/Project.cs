using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace APIEmployeeDepartmentProject.Models
{
    // A class representing a Project in a Company
    // This will be mapped to the SQL table "Porjects"
    public class Project
    {
        // EF Core will automailically treat the "id" primary key in database
        public int id { get; set; }

        // Project name required
        [Required,MaxLength(120)]
        public string name { get; set; }

        // Budget which cannot be negative

        [Range(0,double.MaxValue)]
        public decimal Budget { get; set; }

        // Unique code for the project generated with the use of external api "Codito"

        [Required,MaxLength(50)]
        public string ProjectCode { get; set; }

        // Project can have many employees assigned 
        public ICollection<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
       
        

    }
}

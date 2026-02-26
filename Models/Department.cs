using System.ComponentModel.DataAnnotations;

namespace APIEmployeeDepartmentProject.Models
{
    // A class reporesenting a Department in a company
    // That will be mapped to the SQL table "Departments"
    public class Department
    {
        // EF Core will automailically treat the "id" primary key in database
        public int id { get; set; }

        // API Validation + DB does not allow nulls
        // Sets limits to the size of the string (name)

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100)]
        public string OfficeLocation { get; set; }
    }
}

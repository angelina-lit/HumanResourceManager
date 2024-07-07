using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Employee.Dto
{
    public class EmployeeCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}

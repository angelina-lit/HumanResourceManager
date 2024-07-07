using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DepartmentEmployees.Models.Employee.Dto
{
    public class EmployeeUpdateDTO
    {
        [Required]
        public int Id { get; set; }
		[JsonIgnore]
		[MaxLength(30)]
        public string? FullName { get; set; }
        public string Role { get; set; }
    }
}

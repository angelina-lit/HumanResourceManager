using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Employee.Dto
{
    public class EmployeeCreateDTO
    {
        [Required]
        [MaxLength(30)]
        public string FullName { get; set; }
        public string Role { get; set; }
    }
}

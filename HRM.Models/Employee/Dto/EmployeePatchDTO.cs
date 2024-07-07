using System.ComponentModel.DataAnnotations;

namespace HRM.Models.Employee.Dto
{
    public class EmployeePatchDTO
    {
        [Required]
        public int Id { get; set; }
        public string Role { get; set; }
    }
}

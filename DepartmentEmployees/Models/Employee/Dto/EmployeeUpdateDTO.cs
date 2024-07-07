using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Employee.Dto
{
    public class EmployeeUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(30)]
        public string? FullName { get; set; }
        public string Role { get; set; }
    }
}

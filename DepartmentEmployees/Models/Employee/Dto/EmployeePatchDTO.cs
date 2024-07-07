using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Employee.Dto
{
    public class EmployeePatchDTO
    {
        [Required]
        public int Id { get; set; }
        public string Role { get; set; }
    }
}

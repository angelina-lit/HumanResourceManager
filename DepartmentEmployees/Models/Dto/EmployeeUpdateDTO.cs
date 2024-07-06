using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Dto
{
	public class EmployeeUpdateDTO
	{
		[Required]
		public int Id { get; set; }
		public string? FullName { get; set; }
		[Required]
		[MaxLength(30)]
		public string Post { get; set; }
	}
}

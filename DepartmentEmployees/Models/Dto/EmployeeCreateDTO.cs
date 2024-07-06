using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Dto
{
	public class EmployeeCreateDTO
	{
		[Required]
		[MaxLength(30)]
		public string FullName { get; set; }
		public string Post { get; set; }
	}
}

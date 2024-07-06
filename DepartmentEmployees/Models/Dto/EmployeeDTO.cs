using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DepartmentEmployees.Models.Dto
{
	public class EmployeeDTO
	{
		public int Id { get; set; }
		[Required]
		[MaxLength(30)]
		public string FullName { get; set; }
		public string Post { get; set; }
	}
}

using DepartmentEmployees.Models.Dto;

namespace DepartmentEmployees.Data
{
	public class ListOfEmployees
	{
		public static List<EmployeeDTO> employeeList = new List<EmployeeDTO> {
				new EmployeeDTO { Id = 1, FullName = "Иванов Иван Иванович", Post = "Директор" },
				new EmployeeDTO { Id = 2, FullName = "Петров Петр Петрович", Post = "Бухгалтер" }
				};
	}
}

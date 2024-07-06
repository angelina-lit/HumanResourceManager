using DepartmentEmployees.Models;

namespace DepartmentEmployees.Repository.IRepository
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<Employee> UpdateAsync(Employee entity);
	}
}

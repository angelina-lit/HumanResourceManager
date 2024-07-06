using DepartmentEmployees.Models;

namespace DepartmentEmployees.Repository.IRepository
{
	public interface IEmployeeRepository : IRepository<Employee>
	{
		void Update(Employee obj);
	}
}

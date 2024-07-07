using DepartmentEmployees.Models.Employee;

namespace DepartmentEmployees.DataAccess.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<Employee> UpdateAsync(Employee entity);
	}
}

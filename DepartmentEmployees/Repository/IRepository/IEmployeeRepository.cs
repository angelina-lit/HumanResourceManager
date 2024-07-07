using DepartmentEmployees.Models.Employee;

namespace DepartmentEmployees.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
	{
		Employee Update(Employee entity);
	}
}

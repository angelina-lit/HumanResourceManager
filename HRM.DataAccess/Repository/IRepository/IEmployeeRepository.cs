using HRM.Models.Employee;

namespace HRM.DataAccess.Repository.IRepository
{
    public interface IEmployeeRepository : IRepository<Employee>
	{
		Task<Employee> UpdateAsync(Employee entity);
	}
}

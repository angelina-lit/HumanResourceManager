using DepartmentEmployees.Models;

namespace DepartmentEmployees.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IEmployeeRepository Employee { get; }
	}
}

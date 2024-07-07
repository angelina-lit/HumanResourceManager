namespace DepartmentEmployees.DataAccess.Repository.IRepository
{
	public interface IUnitOfWork
	{
		IEmployeeRepository Employee { get; }

		Task SaveAsync();
	}
}

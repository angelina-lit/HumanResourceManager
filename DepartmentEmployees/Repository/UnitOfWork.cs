using DepartmentEmployees.Data;
using DepartmentEmployees.Repository.IRepository;

namespace DepartmentEmployees.Repository
{
	public class UnitOfWork : IUnitOfWork
	{
		private ApplicationDbContext _db;
		public IEmployeeRepository Employee { get; private set; }

		public UnitOfWork(ApplicationDbContext db)
		{
			_db = db;
			Employee = new EmployeeRepository(_db);
		}

		public async Task SaveAsync()
		{
			await _db.SaveChangesAsync();
		}
	}
}

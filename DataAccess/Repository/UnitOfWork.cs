using DepartmentEmployees.DataAccess.Data;
using DepartmentEmployees.DataAccess.Repository.IRepository;

namespace DepartmentEmployees.DataAccess.Repository
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

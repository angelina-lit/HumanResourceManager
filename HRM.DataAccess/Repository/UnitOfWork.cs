using HRM.DataAccess.Data;
using HRM.DataAccess.Repository.IRepository;

namespace HRM.DataAccess.Repository
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

using HRM.DataAccess.Data;
using HRM.DataAccess.Repository.IRepository;
using HRM.Models.Employee;

namespace HRM.DataAccess.Repository
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
	{
		private readonly ApplicationDbContext _db;
		public EmployeeRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task<Employee> UpdateAsync(Employee entity)
		{
			await Task.Run(() => _db.Employees.Update(entity));
			return entity;
		}
	}
}

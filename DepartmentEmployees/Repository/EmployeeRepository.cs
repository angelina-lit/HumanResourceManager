using DepartmentEmployees.Data;
using DepartmentEmployees.Models;
using DepartmentEmployees.Repository.IRepository;

namespace DepartmentEmployees.Repository
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
			_db.Employees.Update(entity);
			await _db.SaveChangesAsync();
			return entity;
		}
	}
}

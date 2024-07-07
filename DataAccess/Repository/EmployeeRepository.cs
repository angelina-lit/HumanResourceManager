using DepartmentEmployees.DataAccess.Data;
using DepartmentEmployees.DataAccess.Repository.IRepository;
using DepartmentEmployees.Models.Employee;

namespace DepartmentEmployees.DataAccess.Repository
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

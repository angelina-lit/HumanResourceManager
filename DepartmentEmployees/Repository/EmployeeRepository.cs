using DepartmentEmployees.Data;
using DepartmentEmployees.Models;
using DepartmentEmployees.Repository.IRepository;

namespace DepartmentEmployees.Repository
{
	public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
	{
		private ApplicationDbContext _db;
		public EmployeeRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public void Update(Employee obj)
		{
			_db.Employees.Update(obj);
		}
	}
}

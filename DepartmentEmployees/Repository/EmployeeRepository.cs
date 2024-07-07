﻿using DepartmentEmployees.Data;
using DepartmentEmployees.Models.Employee;
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

		/*public Employee Update(Employee entity)
		{
			_db.Employees.Update(entity);
			return entity;
		}*/
		public async Task<Employee> UpdateAsync(Employee entity)
		{
			await Task.Run(() => _db.Employees.Update(entity));
			return entity;
		}
	}
}

using HRM.Models.Employee;
using Microsoft.EntityFrameworkCore;

namespace HRM.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Employee> Employees { get; set; }
	}
}

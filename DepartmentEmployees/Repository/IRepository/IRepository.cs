using System.Linq.Expressions;

namespace DepartmentEmployees.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
		Task CreateAsync(T entity);
		//void Remove(T entity);
		Task RemoveAsync(T entity);
	}
}

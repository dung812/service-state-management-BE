using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionManagement.RepositoryLayer.Interfaces
{
	public interface IBaseRepository<T, I> where T : class
	{
		IQueryable<T> Entities { get; }
		IEnumerable<T> GetAll();
		T Get(I id);
		T Update(T entity);
		T AddAndSave(T entity);
		void Delete(T entity);
		int DeleteRange(IEnumerable<T> entities);
		Task<T> GetAsync(I id);
		Task<IEnumerable<T>> GetAllAsync();
		Task<T> AddAsync(T entity);
		Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity);

		int SaveChanges();
		void ChangeTracking(T entity, EntityState state);
		Task<int> SaveChangesAsync();
	}
}

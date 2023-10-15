using Microsoft.EntityFrameworkCore;
using ProductionManagement.DataAccessLayer.Context;
using ProductionManagement.RepositoryLayer.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductionManagement.RepositoryLayer.Repositories
{
	public class BaseRepository<T, I> : IBaseRepository<T, I> where T : class
	{
		protected readonly ProductionManagementContext _dbContext;

		public IQueryable<T> Entities => _dbContext.Set<T>();

		public BaseRepository(ProductionManagementContext dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual T AddAndSave(T model)
		{
			_dbContext.Set<T>().Add(model);
			SaveChanges();
			return model;
		}

		public virtual T Get(I id)
		{
			return _dbContext.Set<T>().Find(id);
		}

		public virtual IEnumerable<T> GetAll()
		{
			return _dbContext.Set<T>().ToList();
		}

		public virtual T Update(T model)
		{
			//_dbContext.Set<T>().Update(model);
			_dbContext.Entry<T>(model).State = EntityState.Modified;
			SaveChanges();
			return model;
		}

		public void Delete(T model)
		{
			_dbContext.Set<T>().Remove(model);
			SaveChanges();
		}

		public int DeleteRange(IEnumerable<T> models)
		{
			_dbContext.Set<T>().RemoveRange(models);
			return SaveChanges();
		}

		public async Task<T> GetAsync(I id)
		{
			return await _dbContext.Set<T>().FindAsync(id);
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			return await Entities.ToListAsync();
		}

		public async Task<T> AddAsync(T entity)
		{
			await _dbContext.Set<T>().AddAsync(entity);
			await SaveChangesAsync();
			return entity;
		}

		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
		{
			await _dbContext.Set<T>().AddRangeAsync(entity);
			await SaveChangesAsync();
			return entity;
		}

		public int SaveChanges()
		{
			return _dbContext.SaveChanges();
		}

		public async Task<int> SaveChangesAsync()
		{
			return await _dbContext.SaveChangesAsync();
		}

		public void ChangeTracking(T entity, EntityState state)
		{
			_dbContext.Entry<T>(entity).State = state;
		}
	}
}

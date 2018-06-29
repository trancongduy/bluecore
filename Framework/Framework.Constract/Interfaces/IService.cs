using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Constract.Constant;
using Framework.Constract.SeedWork;

namespace Framework.Constract.Interfaces
{
	public interface IService : IDisposable
	{
		IUnitOfWork UnitOfWork { get; }
	}

	public interface IService<TEntity, TDto> : IService where TEntity : BaseEntity where TDto : BaseDto
	{
		IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate);
		IEnumerable<TDto> GetAll();
		Task<IEnumerable<TDto>> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
		PaginatedList<TDto> GetAll(int pageIndex, int pageSize);
		PaginatedList<TDto> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending);
		PaginatedList<TDto> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		IEnumerable<TDto> GetAll<TTypeSelector>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		IEnumerable<TResult> GetAll<TTypeSelector, TResult>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		TDto GetById(Guid id);
		void Add(TDto entity);
		void Update(TDto entity);
		void Delete(TDto entity);
		Task<List<TDto>> GetAllAsync();
		Task<PaginatedList<TDto>> GetAllAsync(int pageIndex, int pageSize);
		Task<PaginatedList<TDto>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending);
		Task<PaginatedList<TDto>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		Task<TDto> GetByIdAsync(Guid id);
		Task<int> AddAsync(TDto entity);
		Task<int> InsertAsync(TDto entity);
		Task<int> UpdateAsync(TDto entity);
		Task<bool> DeleteAsync(Guid id);
		Task<TDto> GetByIdWithInclude(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
	}
}

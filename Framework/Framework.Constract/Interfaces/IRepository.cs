using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Constract.Constant;
using Framework.Constract.SeedWork;

namespace Framework.Constract.Interfaces
{
	public interface IRepository<TEntity> : IDisposable where TEntity : BaseEntity
	{
		IQueryable<TEntity> GetAll();
		PaginatedList<TEntity> GetAll(int pageIndex, int pageSize);
		PaginatedList<TEntity> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending);
		PaginatedList<TEntity> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		List<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
		IQueryable<TEntity> QueryAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);
		TEntity GetSingle(Guid id);
		TEntity GetSingleIncluding(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
		List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
		void Add(TEntity entity);
		void Insert(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
		Task<List<TEntity>> GetAllAsync();
		Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize);
		Task<PaginatedList<TEntity>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending);
		Task<PaginatedList<TEntity>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties);
		IQueryable<TEntity> QueryInclude(params Expression<Func<TEntity, object>>[] includeExpressions);
		Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties);
		Task<TEntity> GetSingleAsync(Guid id);
		Task<TEntity> GetSingleIncludingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
		Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate);

		IQueryable<TEntity> FilterQuery<TTypeSelector>(Expression<Func<TEntity, TTypeSelector>> keySelector,
			Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
			Expression<Func<TEntity, object>>[] includeProperties);
		IEnumerable<TResult> FilterQuery<TTypeSelector, TResult>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, TResult>> selector,
			Expression<Func<TResult, bool>> predicate, OrderBy orderBy,
			Expression<Func<TEntity, object>>[] includeProperties);
	}
}

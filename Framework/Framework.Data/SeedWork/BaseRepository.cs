using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Framework.Common.Extensions;
using Framework.Constract.Constant;
using Framework.Constract.Interfaces;
using Framework.Constract.SeedWork;
using Framework.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Framework.Data
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
	{
        private readonly IBaseContext _context;
		private readonly DbSet<TEntity> _dbSet;
		private bool _disposed;

		public BaseRepository(IBaseContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>();
		}

		public IQueryable<TEntity> GetAll()
		{
			return _dbSet.AsQueryable();
		}

		public PaginatedList<TEntity> GetAll(int pageIndex, int pageSize)
		{
			return GetAll(pageIndex, pageSize, x => x.Id);
		}

		public PaginatedList<TEntity> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending)
		{
			return GetAll(pageIndex, pageSize, keySelector, null, orderBy);
		}

		public PaginatedList<TEntity> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
			var total = entities.Count();// entities.Count() is different than pageSize
			entities = entities.Paginate(pageIndex, pageSize);
			return entities.ToPaginatedList(pageIndex, pageSize, total);
		}

		public List<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			return entities.ToList();
		}
		public IQueryable<TEntity> QueryAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			return entities;
		}
		public TEntity GetSingle(Guid id)
		{
			return _dbSet.FirstOrDefault(t => t.Id == id);
		}

		public TEntity GetSingleIncluding(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			return entities.FirstOrDefault(x => x.Id == id);
		}

		public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbSet.Where(predicate).ToList();
		}

		public void Add(TEntity entity)
		{
			_dbSet.Add(entity);
		}

		public void Insert(TEntity entity)
		{
			_context.SetAsAdded(entity);
		}

		public void Update(TEntity entity)
		{
			//entity.UpdatedDate = DateTime.Now;
			_context.SetAsModified(entity);
		}

		public void Delete(TEntity entity)
		{
			_context.SetAsDeleted(entity);
		}

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
		{
            return _context.SaveChanges(acceptAllChangesOnSuccess);
		}

		public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
		{
            return _context.SaveChangesAsync(cancellationToken = default(CancellationToken));
		}

		public Task<List<TEntity>> GetAllAsync()
		{
			return _dbSet.ToListAsync();
		}

		public Task<PaginatedList<TEntity>> GetAllAsync(int pageIndex, int pageSize)
		{
			return GetAllAsync(pageIndex, pageSize, x => x.Id);
		}

		public Task<PaginatedList<TEntity>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending)
		{
			return GetAllAsync(pageIndex, pageSize, keySelector, null, orderBy);
		}

		public async Task<PaginatedList<TEntity>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector,
			Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = FilterQuery(keySelector, predicate, orderBy, includeProperties);
			var total = await entities.CountAsync();// entities.CountAsync() is different than pageSize
			entities = entities.Paginate(pageIndex, pageSize);
			var list = await entities.ToListAsync();
			return list.ToPaginatedList(pageIndex, pageSize, total);
		}

		public IQueryable<TEntity> QueryInclude(params Expression<Func<TEntity, object>>[] includeExpressions)
		{
			return includeExpressions.Aggregate<Expression<Func<TEntity, object>>, IQueryable<TEntity>>(_dbSet.AsQueryable(),
				(current, expression) => current.Include(expression));
		}

		public Task<List<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			return entities.ToListAsync();
		}

		public Task<TEntity> GetSingleAsync(Guid id)
		{
			return _dbSet.FirstOrDefaultAsync(t => t.Id == id);
		}

		public Task<TEntity> GetSingleIncludingAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			return entities.FirstOrDefaultAsync(x => x.Id == id);
		}

		public Task<List<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate)
		{
			return _dbSet.Where(predicate).ToListAsync();
		}

		public IQueryable<TEntity> FilterQuery<TTypeSelector>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
			Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);
			entities = (predicate != null) ? entities.Where(predicate) : entities;
			entities = (orderBy == OrderBy.Ascending)
				? entities.OrderBy(keySelector)
				: entities.OrderByDescending(keySelector);
			return entities;
		}

		public IEnumerable<TResult> FilterQuery<TTypeSelector, TResult>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, bool>> predicate,
			OrderBy orderBy, Expression<Func<TEntity, object>>[] includeProperties)
		{
			var entities = IncludeProperties(includeProperties);

			entities = (orderBy == OrderBy.Ascending)
				? entities.OrderBy(keySelector)
				: entities.OrderByDescending(keySelector);
			var result = entities.Select(selector).AsEnumerable();
			result = (predicate != null) ? result.Where(predicate.Compile()) : result;
			return result;
		}

		private IQueryable<TEntity> IncludeProperties(params Expression<Func<TEntity, object>>[] includeProperties)
		{
			IQueryable<TEntity> entities = _dbSet;
			foreach (var includeProperty in includeProperties)
			{
				entities = entities.Include(includeProperty);
			}
			return entities;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual void Dispose(bool disposing)
		{
			if (!_disposed && disposing)
			{
				_context.Dispose();
			}
			_disposed = true;
		}
	}
}

using Framework.Constract.Constant;
using Framework.Constract.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Framework.Constract.SeedWork;

namespace Framework.Data.SeedWork
{
    public abstract class Service<TEntity, TDto> : IService<TEntity, TDto> where TEntity : BaseEntity where TDto : BaseDto
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly IRepository<TEntity> _repository;
        private bool _disposed;
        private readonly IMapper _mapper;

        protected Service(IUnitOfWork unitOfWork, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = UnitOfWork.Repository<TEntity>();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate = null)
        {
            return _repository.GetAll().Where(predicate ?? (a => true));
        }
        public IQueryable<TEntity> QueryIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.QueryAllIncluding(includeProperties);
        }
        public IEnumerable<TDto> GetAll()
        {
            return _mapper.MapTo<IEnumerable<TDto>>(_repository.GetAll().AsEnumerable());
        }

        public Task<IEnumerable<TDto>> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(_mapper.MapTo<IEnumerable<TDto>>(_repository.QueryInclude(includeProperties).Where(predicate).AsEnumerable()));
        }

        public PaginatedList<TDto> GetAll(int pageIndex, int pageSize)
        {
            return _mapper.MapTo<PaginatedList<TDto>>(_repository.GetAll(pageIndex, pageSize));
        }

        public PaginatedList<TDto> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return _mapper.MapTo<PaginatedList<TDto>>(_repository.GetAll(pageIndex, pageSize, keySelector, orderBy));
        }

        public PaginatedList<TDto> GetAll<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _mapper.MapTo<PaginatedList<TDto>>(_repository.GetAll(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties));
        }

        public IEnumerable<TDto> GetAll<TTypeSelector>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _mapper.MapTo<IEnumerable<TDto>>(_repository.FilterQuery(keySelector, predicate, orderBy, includeProperties).AsEnumerable());
        }

        public IEnumerable<TResult> GetAll<TTypeSelector, TResult>(Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, TResult>> selector, Expression<Func<TResult, bool>> predicate, OrderBy orderBy,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FilterQuery(keySelector, selector, predicate, orderBy, includeProperties);
        }

        public TDto GetById(Guid id)
        {
            return _mapper.MapTo<TDto>(_repository.GetSingle(id));
        }

        public void Add(TDto dto)
        {
            _repository.Add(_mapper.MapTo<TEntity>(dto));
            UnitOfWork.SaveChanges();
        }

        public void Update(TDto dto)
        {
            var entity = _repository.GetSingle(dto.Id);
            dto.CreatedDate = entity.CreatedDate;
            dto.CreatedBy = entity.CreatedBy;
            entity = _mapper.MapToInstance(dto, entity);
            _repository.Update(_mapper.MapToInstance(dto, entity));
            UnitOfWork.SaveChanges();
        }

        public void Delete(TDto dto)
        {
            _repository.Delete(_mapper.MapTo<TEntity>(dto));
            UnitOfWork.SaveChanges();
        }

        public Task<List<TDto>> GetAllAsync()
        {
            var result = _repository.GetAllAsync().Result;
            return Task.FromResult(_mapper.MapTo<List<TDto>>(result));
        }

        public Task<PaginatedList<TDto>> GetAllAsync(int pageIndex, int pageSize)
        {
            return Task.FromResult(_mapper.MapTo<PaginatedList<TDto>>(_repository.GetAllAsync(pageIndex, pageSize).Result));
        }

        public Task<PaginatedList<TDto>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, OrderBy orderBy = OrderBy.Ascending)
        {
            return Task.FromResult(_mapper.MapTo<PaginatedList<TDto>>(_repository.GetAllAsync(pageIndex, pageSize, keySelector, orderBy)));
        }

        public Task<PaginatedList<TDto>> GetAllAsync<TTypeSelector>(int pageIndex, int pageSize, Expression<Func<TEntity, TTypeSelector>> keySelector, Expression<Func<TEntity, bool>> predicate, OrderBy orderBy, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(_mapper.MapTo<PaginatedList<TDto>>(_repository.GetAllAsync(pageIndex, pageSize, keySelector, predicate, orderBy, includeProperties).Result));
        }

        public Task<TDto> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_mapper.MapTo<TDto>(_repository.GetSingleAsync(id).Result));
        }

        public Task<int> AddAsync(TDto dto)
        {
            _repository.Add(_mapper.MapTo<TEntity>(dto));
            return UnitOfWork.SaveChangesAsync();
        }
        public async Task<Guid> AddReturnIdAsync(TDto dto)
        {
            var entity = _mapper.MapTo<TEntity>(dto);
            _repository.Add(entity);
            var result = await UnitOfWork.SaveChangesAsync();
            return entity.Id;
        }
        public Task<int> InsertAsync(TDto dto)
        {
            _repository.Insert(_mapper.MapTo<TEntity>(dto));
            return UnitOfWork.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(TDto dto)
        {
            var entity = _repository.GetSingle(dto.Id);
            dto.CreatedBy = entity.CreatedBy;
            dto.CreatedDate = entity.CreatedDate;
            entity = _mapper.MapToInstance(dto, entity);
            _repository.Update(entity);
            return UnitOfWork.SaveChangesAsync();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = _repository.GetSingle(id);
                _repository.Delete(entity);
                var i = UnitOfWork.SaveChangesAsync().Result;
                if (i > 0)
                {
                    return Task.FromResult(true);
                }
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(false);
        }

        public Task<TDto> GetByIdWithInclude(Guid id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Task.FromResult(_mapper.MapTo<TDto>(_repository.GetSingleIncluding(id, includeProperties)));
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
                UnitOfWork.Dispose();
            }
            _disposed = true;
        }
    }
}

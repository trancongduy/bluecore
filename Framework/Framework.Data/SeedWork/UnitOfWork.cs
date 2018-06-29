using Framework.Constract.Interfaces;
using Framework.Data.Interfaces;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using Framework.Constract.SeedWork;

namespace Framework.Data.SeedWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IBaseContext _baseContext;
        private Hashtable _repositories;
        private bool _disposed;

        public UnitOfWork(IBaseContext baseContext)
        {
            _baseContext = baseContext;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
            {
                return (IRepository<TEntity>)_repositories[type];
            }
            var repositoryType = typeof(BaseRepository<>);
            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _baseContext));
            return (IRepository<TEntity>)_repositories[type];
        }

        public int SaveChanges()
        {
            return _baseContext.SaveChanges();
        }

        public Task<int> SaveChangesAsync()
        {
            return _baseContext.SaveChangesAsync();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return _baseContext.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void BeginTransactionSelf()
        {
            throw new NotImplementedException();
        }

        public int Commit()
        {
            throw new NotImplementedException();
        }

        public Task<int> CommitAsync()
        {
            throw new NotImplementedException();
        }

        public void CommitTransactionSelf()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
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
                _baseContext.Dispose();
                foreach (IDisposable repository in _repositories.Values)
                {
                    repository.Dispose();// dispose all repositries
                }
            }
            _disposed = true;
        }
    }
}

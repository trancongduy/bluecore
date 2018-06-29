using System;
using System.Threading;
using System.Threading.Tasks;
using Framework.Constract.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Framework.Data.Interfaces
{
    public interface IBaseContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        void SetAsAdded<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void SetAsModified<TEntity>(TEntity entity) where TEntity : BaseEntity;
        void SetAsDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity;
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));
        void BeginTransaction();
        int Commit();
        void Rollback();
        Task<int> CommitAsync();
        void BeginTransactionSelf();
        void CommitTransactionSelf();
        void ExecuteCommand(string command, params object[] parameters);
    }
}

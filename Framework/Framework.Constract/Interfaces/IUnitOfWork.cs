using System;
using System.Threading;
using System.Threading.Tasks;
using Framework.Constract.SeedWork;

namespace Framework.Constract.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		int SaveChanges();
		IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
		void BeginTransaction();
		int Commit();
		void Rollback();
		Task<int> SaveChangesAsync();
		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
		Task<int> CommitAsync();
		void CommitTransactionSelf();
		void BeginTransactionSelf();
	}
}

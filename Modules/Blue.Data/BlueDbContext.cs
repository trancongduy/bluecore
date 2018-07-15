using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Framework.Data.Interfaces;
using Framework.Data.Models;
using Framework.Constract.SeedWork;
using Microsoft.EntityFrameworkCore.Storage;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;
using Framework.Constract.Constant;
using Framework.Constract.Interfaces;
using Framework.Data.SeedWork;

namespace Blue.Data
{
    public class BlueDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>, IBaseContext
    {
        private IDbContextTransaction _dbContextTransaction;
        private DbTransaction _transaction;
        private static bool _databaseInitialized;
        private static readonly object Lock = new object();
        private readonly IUserResolverService _userResolverService;

        public BlueDbContext(DbContextOptions<BlueDbContext> options, IUserResolverService userResolverService) : base(options)
        {
            _userResolverService = userResolverService;

            if (_databaseInitialized)
            {
                return;
            }
            lock (Lock)
            {
                if (!_databaseInitialized)
                {
                    // Set the database intializer which is run once during application start
                    // This seeds the database with admin user credentials and admin role
                    //Database.SetInitializer(new ApplicationDbInitializer());
                    _databaseInitialized = true;
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var typeToRegisters = new List<Type>();
            foreach (var module in GlobalConfiguration.Modules)
            {
                typeToRegisters.AddRange(module.Assembly.DefinedTypes.Select(t => t.AsType()));
            }

            RegisterEntities(builder, typeToRegisters);

            RegisterConvention(builder);

            base.OnModelCreating(builder);

            RegisterCustomMappings(builder, typeToRegisters);
            //modelBuilder.AddEntityConfigurationsFromAssembly(GetType().Assembly);
        }

        private static void RegisterEntities(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var entityTypes = typeToRegisters.Where(x => x.GetTypeInfo().IsSubclassOf(typeof(BaseEntity)) && !x.GetTypeInfo().IsAbstract);
            foreach (var type in entityTypes)
            {
                modelBuilder.Entity(type);
            }
        }

        private static void RegisterConvention(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                if (entity.ClrType.Namespace != null)
                {
                    var tableName = entity.ClrType.Name;
                    modelBuilder.Entity(entity.Name).ToTable(tableName);
                }
            }
        }

        private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        {
            var customModelBuilderTypes = typeToRegisters.Where(x => typeof(ICustomModelBuilder).IsAssignableFrom(x));
            foreach (var builderType in customModelBuilderTypes)
            {
                if (builderType != null && builderType != typeof(ICustomModelBuilder))
                {
                    var builder = (ICustomModelBuilder)Activator.CreateInstance(builderType);
                    builder.Build(modelBuilder);
                }
            }
        }

        //private static void RegisterCustomMappings(ModelBuilder modelBuilder, IEnumerable<Type> typeToRegisters)
        //{
        //    var customModelBuilderTypes = typeToRegisters.Where(x => typeof(IEntityMappingConfiguration).IsAssignableFrom(x));
        //    foreach (var builderType in customModelBuilderTypes)
        //    {
        //        if (builderType != null && builderType != typeof(IEntityMappingConfiguration))
        //        {
        //            modelBuilder.AddEntityConfigurationsFromAssembly(builderType.Assembly);
        //        }
        //    }
        //}

        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        public void SetAsAdded<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Added);
        }

        public void SetAsModified<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Modified);
        }

        public void SetAsDeleted<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            UpdateEntityState(entity, EntityState.Deleted);
        }

        public void BeginTransaction()
        {
            if (Database.GetDbConnection().State == ConnectionState.Open)
            {
                return;
            }
            Database.GetDbConnection().Open();
            _transaction = Database.GetDbConnection().BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public int Commit()
        {
            var saveChanges = SaveChanges();
            try
            {
                if (saveChanges > 0)
                {
                    _transaction.Commit();
                }
                else
                {
                    _transaction.Rollback();
                }

            }
            catch (Exception)
            {
                _transaction.Rollback();
            }

            return saveChanges;
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task<int> CommitAsync()
        {
            var saveChangeAsync = await SaveChangesAsync();
            if (saveChangeAsync > 0)
            {
                _transaction.Commit();
            }
            return saveChangeAsync;
        }

        public void BeginTransactionSelf()
        {
            _dbContextTransaction = Database.BeginTransaction();
        }

        public void CommitTransactionSelf()
        {
            var result = SaveChanges();
            if (result > 0)
            {
                _dbContextTransaction.Commit();
            }
            else
            {
                _dbContextTransaction.Rollback();
            }
        }

        private void UpdateEntityState<TEntity>(TEntity entity, EntityState entityState) where TEntity : BaseEntity
        {
            var dbEntityEntry = GetDbEntityEntrySafely(entity);
            dbEntityEntry.State = entityState;
        }

        private EntityEntry GetDbEntityEntrySafely<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            var dbEntityEntry = Entry(entity);

            if (dbEntityEntry.State == EntityState.Detached)
            {
                Set<TEntity>().Attach(entity);
            }
            //else if (dbEntityEntry.State == EntityState.Modified)
            //{
            //    var props = entity.GetType().GetProperties();
            //    foreach (var prop in props)
            //    {
            //        object value = prop.GetValue(entity);
            //        if (prop.PropertyType.IsInterface && value != null)
            //        {
            //            foreach (var iItem in (System.Collections.IEnumerable)value)
            //            {
            //                var resValue = iItem.GetType().GetProperty("Id")?.GetValue(iItem);
            //                if (resValue == null) continue;

            //                var id = (int)resValue;
            //                if (id == 0)
            //                {
            //                    Entry(iItem).State = EntityState.Added;
            //                }
            //                else
            //                {
            //                    Entry(iItem).State = EntityState.Detached;
            //                }
            //            }
            //        }
            //    }
            //}

            return dbEntityEntry;
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            Database.ExecuteSqlCommand(command, parameters);
        }

		public override int SaveChanges()
		{
            Audit();

		    var validationErrors = ChangeTracker
		        .Entries<IValidatableObject>()
		        .SelectMany(e => e.Entity.Validate(new ValidationContext(e.Entity)))
		        .Where(r => r != ValidationResult.Success);

		    if (validationErrors.Any())
		    {
		        // Possibly throw an exception here
		    }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Audit();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            Audit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public void Audit()
        {
            var entityTracks = ChangeTracker
                .Entries()
                .Where(x => x.State == EntityState.Modified || x.State == EntityState.Added ||
                       x.State == EntityState.Deleted && x.Entity is BaseEntity);

            var currentUser = _userResolverService.GetUser();

            foreach (var entity in entityTracks)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        ((IAuditableEntity) entity.Entity).CreatedDate = DateTimeOffset.Now;
                        ((IAuditableEntity) entity.Entity).CreatedBy =
                            !string.IsNullOrEmpty(((IAuditableEntity) entity.Entity).CreatedBy)
                                ? currentUser
                                : UserType.SystemGenerated;
                        ((IAuditableEntity) entity.Entity).IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        ((IAuditableEntity) entity.Entity).UpdatedDate = DateTimeOffset.Now;
                        ((IAuditableEntity) entity.Entity).UpdatedBy = currentUser;
                        break;
                    case EntityState.Deleted:
                        ((IAuditableEntity) entity.Entity).UpdatedBy = currentUser;
                        ((IAuditableEntity) entity.Entity).IsDeleted = true;
                        ((IAuditableEntity) entity.Entity).UpdatedDate = DateTimeOffset.Now;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

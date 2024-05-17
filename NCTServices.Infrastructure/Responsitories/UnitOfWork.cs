using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Domain.Contracts;
using NCTServices.Infrastructure.Contexts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Infrastructure.Responsitories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NCTServicesDBReadContext _dbReadContext;
        private readonly NCTServicesDBWriteContext _dbWriteContext;
        private bool disposed;
        private Hashtable _repositories;
        public UnitOfWork(NCTServicesDBReadContext dbReadContext, NCTServicesDBWriteContext dbWriteContext)
        {
            _dbReadContext = dbReadContext ?? throw new ArgumentNullException(nameof(dbReadContext));
            _dbWriteContext = dbWriteContext ?? throw new ArgumentNullException(nameof(dbWriteContext));
        }

        public IRepositoryAsync<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if (_repositories == null)
                _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(ResponsitoryAsync<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dbReadContext, _dbWriteContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IRepositoryAsync<TEntity>)_repositories[type];
        }

        public async Task<int> ComitAndRemoveCache(CancellationToken cancellationToken, string cacheKey)
        {
            var result = await _dbWriteContext.SaveChangesAsync(cancellationToken);
            return result;
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbWriteContext.SaveChangesAsync(cancellationToken);
        }

        public Task Rollback()
        {
            _dbWriteContext.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                    _dbWriteContext.Dispose();
                    _dbReadContext.Dispose();
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }
    }
}

using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Contracts.Interfaces.Responsitories
{
    public interface IUnitOfWork :IDisposable
    {
        IRepositoryAsync<T> Repository<T>() where T : BaseEntity;
        Task<int> Commit(CancellationToken cancellationToken);

        Task<int> ComitAndRemoveCache(CancellationToken cancellationToken, string cacheKey);

        Task Rollback();
    }
}

using NCTServices.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NCTServices.Contracts.Interfaces.Responsitories
{
    public interface IRepositoryAsync<T> where T : class, IEntity
    {
        IQueryable<T> Entities { get; }
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllWithFilterAsync(Expression<Func<T, bool>>[] filters);
        Task<List<TResult>> GetAllWithFilterAsync<TResult>(IsolationLevel isolationLevel, IQueryable<T> query, Expression<Func<T, TResult>> selector);
        Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize);
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteRangeAsync(List<T> entities);
    }
}

using Microsoft.EntityFrameworkCore;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Domain.Contracts;
using NCTServices.Infrastructure.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NCTServices.Infrastructure.Responsitories
{
    public class ResponsitoryAsync<T> : IRepositoryAsync<T> where T : BaseEntity
    {
        private readonly NCTServicesDBReadContext _dbReadContext;
        private readonly NCTServicesDBWriteContext _dbWriteContext;

        public ResponsitoryAsync(NCTServicesDBReadContext dbReadContext, NCTServicesDBWriteContext dbWriteContext)
        {
            _dbReadContext = dbReadContext;
            _dbWriteContext = dbWriteContext;
        }

        public IQueryable<T> Entities => _dbReadContext.Set<T>();

        public async Task<T> AddAsync(T entity)
        {
            await _dbWriteContext.Set<T>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(T entity)
        {
            _dbWriteContext.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public Task DeleteRangeAsync(List<T> entity)
        {
            _dbWriteContext.Set<T>().RemoveRange(entity);
            return Task.CompletedTask;
        }


        public async Task<List<T>> GetAllAsync()
        {
            return await _dbReadContext
                .Set<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbReadContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllWithFilterAsync(Expression<Func<T, bool>>[] filters)
        {
            IQueryable<T> query = _dbReadContext.Set<T>();
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
            return query.ToList();
        }

        public async Task<List<TResult>> GetAllWithFilterAsync<TResult>(IsolationLevel isolationLevel, IQueryable<T> query, Expression<Func<T, TResult>> selector)
        {
            var result = new List<TResult>();

            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = isolationLevel }, TransactionScopeAsyncFlowOption.Enabled))
            {
                result = await query.AsNoTracking().Select(selector).ToListAsync();
                scope.Complete();
            }

            return result;
        }

        public async Task<List<T>> GetPagedReponseAsync(int pageNumber, int pageSize)
        {
            return await _dbReadContext
                .Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public Task UpdateAsync(T entity)
        {
            T exist = _dbWriteContext.Set<T>().Find(entity.RowId);
            _dbWriteContext.Entry(exist).CurrentValues.SetValues(entity);
            return Task.CompletedTask;

        }
    }
}

using Dapper;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Contracts.Interfaces.Responsitories
{
    public interface IApplicationReadDbConnection
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> QueryFirstOrDefaultAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);

        Task<T> ExecuteAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<PaginatedResult<T>> QueryWithPagingAsync<T>(string sql, CommandType command, DynamicParameters param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default, int? pageNumber = null, int? pageSize = null);
    }
}

using Dapper;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Infrastructure.Responsitories
{
    public class ApplicationWriteDbConnection : IApplicationWriteDbConnection
    {
        private readonly IApplicationDbContext _applicationDBContext;
        public ApplicationWriteDbConnection(IApplicationDbContext applicationDBContext)
        {
            _applicationDBContext = applicationDBContext;
        }

        public async Task<int> ExecuteAsync(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _applicationDBContext.Connection.ExecuteAsync(sql, param, transaction, null, command);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await _applicationDBContext.Connection.QueryAsync<T>(sql, param, transaction, commandTimeout: 90, command)).AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _applicationDBContext.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, command);
        }

        public async Task<T> ExecuteAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await _applicationDBContext.Connection.QuerySingleAsync<T>(sql, param, transaction, null, command);
        }

        public Task<PaginatedResult<T>> QueryWithPagingAsync<T>(string sql, CommandType command, DynamicParameters param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default, int? pageNumber = null, int? pageSize = null)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertAsync<T>(T param, List<string>? exceptFields = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            if (param == null) return 0;
            if (exceptFields == null) exceptFields = new List<string>();

            var entityType = typeof(T);
            var properties = entityType.GetProperties().Where(a => !exceptFields.Contains(a.Name));
            if (!properties.Any()) return 0;

            var insertColumns = properties.Select(a => $"[{a.Name}]");
            var insertValues = properties.Select(a => $"@{a.Name}");
            var insertSql = $"INSERT INTO {entityType.Name} ({string.Join(',', insertColumns)}) VALUES ({string.Join(',', insertValues)}); SELECT CAST(SCOPE_IDENTITY() as int);";

            var parameters = new DynamicParameters();
            parameters.AddDynamicParams(param);

            return await _applicationDBContext.Connection.QuerySingleAsync<int>(insertSql, parameters, transaction, commandType: CommandType.Text);
        }
    }
}

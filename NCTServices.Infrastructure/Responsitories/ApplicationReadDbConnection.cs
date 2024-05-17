using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Shared.Configurations;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Infrastructure.Responsitories
{
    public class ApplicationReadDbConnection : IApplicationReadDbConnection, IDisposable
    {
        private readonly IDbConnection connection;
        //private readonly eServicesDBReadContext _dbReadContext;
        public ApplicationReadDbConnection(IOptions<DatabaseConnectionConfiguration> options)//eServicesDBReadContext dbReadContext)
        {
            //_dbReadContext = dbReadContext;
            //connection = new SqlConnection(_dbReadContext.Connection.ConnectionString);
            connection = new SqlConnection(options.Value.DBConnection);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return (await connection.QueryAsync<T>(sql, param, transaction, commandTimeout: 90, command)).AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, command);
        }

        public async Task<T> ExecuteAsync<T>(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction, null, command);
        }

        public async Task<PaginatedResult<T>> QueryWithPagingAsync<T>(string sql, CommandType command, DynamicParameters param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default, int? pageNumber = null, int? pageSize = null)
        {
            pageNumber = pageNumber == null && pageNumber <= 0 ? 1 : pageNumber;
            pageSize = pageSize == null && pageSize == 0 ? 10 : pageSize;
            param.Add("@Skip", (pageNumber - 1) * pageSize);
            param.Add("@Take", pageSize);

            var query = await connection.QueryMultipleAsync(sql, param, transaction, null, command);
            int count = query.ReadFirst<int>();
            List<T> datas = query.Read<T>().ToList();
            var result = new PaginatedResult<T>(datas);
            result.TotalCount = count;
            result.PageSize = (int)pageSize;
            result.CurrentPage = (int)pageNumber;
            return result;
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

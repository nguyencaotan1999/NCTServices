using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Contracts.Interfaces.Responsitories
{
    public interface IApplicationWriteDbConnection : IApplicationReadDbConnection
    {
        Task<int> ExecuteAsync(string sql, CommandType command, object param = null, IDbTransaction transaction = null, CancellationToken cancellationToken = default);
        Task<int> InsertAsync<T>(T param, List<string>? exceptFields = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
    }
}

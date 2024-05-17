using Dapper;
using MediatR;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Shared.Constants;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Application.Common.Services.Product.Commands
{
    public class DeleteProductById : IRequest<Result<bool>>
    {
        public int RowId { get; set; }
        public DeleteProductById( int Rowid) 
        {
            RowId = Rowid;
        }
    }
    public class DeleteProductByIdHandler : IRequestHandler<DeleteProductById, Result<bool>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public DeleteProductByIdHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }
        public async Task<Result<bool>> Handle(DeleteProductById request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RowId", request.RowId);
                var DeleteProd = await _sqlDbConnection.QueryAsync<bool>(SQLConstant.DELETE_PRODUCT_BYID, CommandType.Text, parameters);
                return await Result<bool>.SuccessAsync(DeleteProd);
            }
            catch (Exception ex)
            {

                return await Result<bool>.FailAsync(ex.Message);
            }
        }
    }
}

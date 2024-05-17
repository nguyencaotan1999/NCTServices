using Dapper;
using MediatR;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Domain.Entity;
using NCTServices.Shared.Constants;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Application.Common.Services.OrderDetail.Commands
{
    public class DeleteOrderDetailCommands : IRequest<Result<bool>>
    {
        public int OrderId { get; set; }
        public DeleteOrderDetailCommands(int orderId) 
        {
            OrderId = orderId;
        }  
    }
    public class DeleteOrderDetailCommandsHandler : IRequestHandler<DeleteOrderDetailCommands, Result<bool>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public DeleteOrderDetailCommandsHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }

        public async Task<Result<bool>> Handle(DeleteOrderDetailCommands request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.OrderId != null)
                {
                    DynamicParameters parameters = new DynamicParameters();
                    parameters.Add("RowId", request.OrderId);
                    await _sqlDbConnection.QueryAsync<OrderDetails>(SQLConstant.DELETE_ORDERDETAIL_BY_ID, CommandType.Text, parameters);
                    return await Result<bool>.SuccessAsync(true);
                }
                else
                { 
                    return await Result<bool>.FailAsync();
                }    
               
            }
            catch (Exception)
            {
                return await Result<bool>.FailAsync();
            }        
        }
    }
}

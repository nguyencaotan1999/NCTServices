using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Domain.Entity;
using NCTServices.Model.Requests;
using NCTServices.Model.Responses;
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
    public class AddOrUpdateOrderDetailCommands : IRequest<Result<bool>>
    {
        public OrderDetailRequest orderDetailRequest { get; set; }
        public AddOrUpdateOrderDetailCommands(OrderDetailRequest request)
        { 
         orderDetailRequest = request;
        }
    }
    public class AddOrUpdateOrderDetailCommandsHandler : IRequestHandler<AddOrUpdateOrderDetailCommands, Result<bool>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        private readonly IUnitOfWork _unitOfWork;
        public AddOrUpdateOrderDetailCommandsHandler(IApplicationWriteDbConnection sqlDbConnection, IUnitOfWork unitOfWork)
        {
            _sqlDbConnection = sqlDbConnection;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(AddOrUpdateOrderDetailCommands request, CancellationToken cancellationToken)
        {
            try
            {
                var GetOrderDetailId = await _unitOfWork.Repository<OrderDetails>().Entities.Where(X => X.RowId == request.orderDetailRequest.RowId).FirstOrDefaultAsync();
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RowId", request.orderDetailRequest.RowId);
                parameters.Add("Quantity", request.orderDetailRequest.Quantity);
                parameters.Add("UnitPrice", request.orderDetailRequest.UnitPrice);
                parameters.Add("Subtotal", request.orderDetailRequest.Subtotal);
                if (GetOrderDetailId != null)
                {
                    await _sqlDbConnection.QueryAsync<OrderDetails>(SQLConstant.Update_ORDERDETAIL, CommandType.Text, parameters);
                }
                else
                {
                    await _sqlDbConnection.QueryAsync<OrderDetails>(SQLConstant.ADD_ORDERDETAIL, CommandType.Text, parameters);
                }
                return await Result<bool>.SuccessAsync(true);
            }
            catch (Exception)
            {
                return await Result<bool>.FailAsync();
            }
        }
    }
}

using Dapper;
using MediatR;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Model.Responses;
using NCTServices.Shared.Constants;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Application.Common.Services.OrderDetail.Queries
{
    public class GetOrderDetailById : IRequest<Result<OrderDetailResponses>>
    {
        public int? RowId { get; set; }
        public GetOrderDetailById(int? rowid)
        {
            RowId = rowid;
        }
    }
    public class GetOrderDetailByIdHandler : IRequestHandler<GetOrderDetailById, Result<OrderDetailResponses>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public GetOrderDetailByIdHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }

        public async Task<Result<OrderDetailResponses>> Handle(GetOrderDetailById request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RowId", request.RowId);

                var reponsesData = await _sqlDbConnection.QueryFirstOrDefaultAsync<OrderDetailResponses>(SQLConstant.GET_ORDERDETAIL_BY_ID, CommandType.Text, parameters);
                OrderDetailResponses response = new OrderDetailResponses();
                if (reponsesData != null)
                {
                    response.OrderDate = reponsesData.OrderDate;
                    response.ProductName = reponsesData.ProductName;
                    response.CustomerName = reponsesData.CustomerName;
                    response.Quantity = reponsesData.Quantity;
                    response.UnitPrice = reponsesData.UnitPrice;

                }

                return await Result<OrderDetailResponses>.SuccessAsync(response);
            }
            catch (Exception)
            {
                return await Result<OrderDetailResponses>.FailAsync();

            }
        }
    }
}

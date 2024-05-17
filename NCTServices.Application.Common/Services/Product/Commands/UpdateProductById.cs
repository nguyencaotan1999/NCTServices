using Dapper;
using MediatR;
using NCTServices.Application.Common.Services.Product.Queries;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Model.Requests;
using NCTServices.Model.Responses;
using NCTServices.Shared.Constants;
using NCTServices.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NCTServices.Application.Common.Services.Product.Commands
{
    public class UpdateProductById : IRequest<Result<bool>>
    {
        public ProductRequest product { get; set; }
        public UpdateProductById(ProductRequest productRequest)
        {
            product = productRequest;
        }
    }
    public class UpdateProductByIdHandler : IRequestHandler<UpdateProductById, Result<bool>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public UpdateProductByIdHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }
        public async Task<Result<bool>> Handle(UpdateProductById request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("RowId", request.product.RowId);
                parameters.Add("ProductName", request.product.ProductName);
                parameters.Add("ProductDescription", request.product.ProductDescription);
                parameters.Add("Price", request.product.Price);
                var UpdateProd = await _sqlDbConnection.QueryAsync<bool>(SQLConstant.Update_Product_By_Id, CommandType.Text, parameters);
                return await Result<bool>.SuccessAsync(UpdateProd);
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailAsync(ex.Message);
            }

        }
    }
}

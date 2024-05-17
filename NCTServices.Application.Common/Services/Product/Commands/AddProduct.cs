using Dapper;
using MediatR;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Model.Requests;
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
    public class AddProduct : IRequest<Result<bool>>
    {
        public ProductRequest product { get; set; }
        public AddProduct(ProductRequest productRequest) 
        {
            product = productRequest;
        }
    }

    public class AddProductHandler : IRequestHandler<AddProduct, Result<bool>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public AddProductHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }
        public async Task<Result<bool>> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("ProductName", request.product.ProductName);
                parameters.Add("ProductDescription", request.product.ProductDescription);
                parameters.Add("Price", request.product.Price);
                parameters.Add("QuantityInStock", request.product.QuantityInStock);
                parameters.Add("ModifyDate", DateTime.Now);
                parameters.Add("CreateDate", DateTime.Now);

                var AddNewProduct = await _sqlDbConnection.QueryAsync<bool>(SQLConstant.Add_PRODUCT, CommandType.Text, parameters);
                return await Result<bool>.SuccessAsync(AddNewProduct);

            }
            catch (Exception ex)
            {
                return await Result<bool>.FailAsync(ex.Message);
                throw;
            }
        }
    }
}

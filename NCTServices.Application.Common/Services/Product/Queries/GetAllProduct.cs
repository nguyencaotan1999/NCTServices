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

namespace NCTServices.Application.Common.Services.Product.Queries
{
    public class GetAllProduct : IRequest<PaginatedResult<ProductResponses>>
    {
        public int? SKiP { get; set; }
        public GetAllProduct(int? skip) 
        { 
        SKiP = skip;
        }
    }
    public class GetAllProductHandler : IRequestHandler<GetAllProduct, PaginatedResult<ProductResponses>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public GetAllProductHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }

        public async Task<PaginatedResult<ProductResponses>> Handle(GetAllProduct request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("Skip", request.SKiP);
                var getLinkPointBenefit = await _sqlDbConnection.QueryAsync<ProductResponses>(SQLConstant.Get_All_Products, CommandType.Text, parameters);
                return PaginatedResult<ProductResponses>.Success(
                    data: (List<ProductResponses>)getLinkPointBenefit,
                    count: getLinkPointBenefit.Count(),
                    page: 1,
                    pageSize: 8,
                    filteredCount: 0
                );
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}

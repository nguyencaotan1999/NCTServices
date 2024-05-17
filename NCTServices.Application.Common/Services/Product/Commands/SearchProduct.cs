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

namespace NCTServices.Application.Common.Services.Product.Commands
{
    public class SearchProduct: IRequest<PaginatedResult<ProductResponses>>
    {
        public string? searchValue { get; set; }
        public int? Skip { get; set; }
        public SearchProduct(string? searchvalue, int? skip) { 
            searchValue = searchvalue;
            Skip = skip;
        }
    }
    public class SearchProductHandler : IRequestHandler<SearchProduct, PaginatedResult<ProductResponses>>
    {
        private readonly IApplicationWriteDbConnection _sqlDbConnection;
        public SearchProductHandler(IApplicationWriteDbConnection sqlDbConnection)
        {
            _sqlDbConnection = sqlDbConnection;
        }
        public async Task<PaginatedResult<ProductResponses>> Handle(SearchProduct request, CancellationToken cancellationToken)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("searchValue", request.searchValue);
                parameters.Add("Skip", request.Skip);
                var getLinkPointBenefit = await _sqlDbConnection.QueryAsync<ProductResponses>(SQLConstant.SEARCH_PRODUCT, CommandType.Text, parameters);
                var mess = SQLConstant.SEARCH_PRODUCT;
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

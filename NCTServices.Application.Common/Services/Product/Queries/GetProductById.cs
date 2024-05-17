using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NCTServices.Contracts.Interfaces.Responsitories;
using NCTServices.Domain.Entity;
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
    public class GetProductById : IRequest<Result<ProductResponses>>
    {
        public int Id { get; set; }
        public GetProductById(int Rowid) 
        {
            Id = Rowid;
        }
    }
    public class GetProductByIdHandler : IRequestHandler<GetProductById, Result<ProductResponses>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetProductByIdHandler(IUnitOfWork unitOfWorks)
        {
            unitOfWork = unitOfWorks;
        }

        public async Task<Result<ProductResponses>> Handle(GetProductById request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await unitOfWork.Repository<Products>().Entities.Where(x => x.RowId == request.Id).FirstOrDefaultAsync();
                ProductResponses productResponses = new ProductResponses();
                if (response != null)
                { 
                    productResponses.RowId = response.RowId;
                    productResponses.ProductName = response.ProductName;
                    productResponses.ProductDescription = response.ProductDescription;
                    productResponses.ProductPrice = response.ProductPrice;
                    productResponses.QuantityInStock = response.QuantityInStock;
                    productResponses.CreatedDate = response.CreatedDate;
                    productResponses.ModifiedDate = response.ModifiedDate;
                }
                return await Result<ProductResponses>.SuccessAsync(productResponses);
            }
            catch (Exception)
            {
                return await Result<ProductResponses>.FailAsync();
            }

        }
    }
}

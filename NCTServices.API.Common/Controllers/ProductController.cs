using Microsoft.AspNetCore.Mvc;
using NCTServices.Application.Common.Services.Product.Commands;
using NCTServices.Application.Common.Services.Product.Queries;
using NCTServices.Model.Requests;


namespace NCTServices.API.Common.Controllers
{
    public class ProductController : BaseApiController<ProductController>
    {

        [HttpGet]
        [Route("ProductById")]
        public async Task<IActionResult> GetProductById(int Rowid)
        {
            try
            {
                    var listProducts = await _mediator.Send(new GetProductById(Rowid));
                    return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }

        [HttpGet]
        [Route("Product")]
        public async Task<IActionResult> GetProducts(string? searchValue, int? skip)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(searchValue))
                {
                    var listProducts = await _mediator.Send(new SearchProduct(searchValue, skip));
                    return Ok(listProducts.Data);
                }
                else
                {
                    var listProducts = await _mediator.Send(new GetAllProduct(skip));
                    return Ok(listProducts.Data);
                }
               
            }
            catch (Exception)
            {
                return BadRequest();            }
            
        }
        [HttpPost]
        [Route("Product")]
        public async Task<IActionResult> AddNewProduct(ProductRequest request)
        {
            try
            {
                var listProducts = await _mediator.Send(new AddProduct(request));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();            }

        }
        [HttpPut]
        [Route("Product")]
        public async Task<IActionResult> UpdateProductsById(ProductRequest request)
        {
            try
            {
                var listProducts = await _mediator.Send(new UpdateProductById(request));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();            }

        }
        [HttpDelete]
        [Route("Product")]
        public async Task<IActionResult> DeleteProductsById(int RowId)
        {
            try
            {
                var listProducts = await _mediator.Send(new DeleteProductById(RowId));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();            }

        }
    }
}

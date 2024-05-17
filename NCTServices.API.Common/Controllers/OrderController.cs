using Microsoft.AspNetCore.Mvc;
using NCTServices.Application.Common.Services.OrderDetail.Commands;
using NCTServices.Application.Common.Services.OrderDetail.Queries;
using NCTServices.Model.Requests;

namespace NCTServices.API.Common.Controllers
{
    public class OrderController : BaseApiController<OrderController>
    {
        [HttpGet]
        [Route("OrderDetail")]
        public async Task<IActionResult> GetOrderdetail(int UserId)
        {
            try
            {
                var listProducts = await _mediator.Send(new GetOrderDetailById(UserId));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }

        [HttpPost]
        [Route("AddOrUpdateOrderDetail")]
        public async Task<IActionResult> AddOrUpdateOrderDetail(OrderDetailRequest OrderDetail)
        {
            try
            {
                var listProducts = await _mediator.Send(new AddOrUpdateOrderDetailCommands(OrderDetail));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }

        [HttpDelete]
        [Route("OrderDetail")]
        public async Task<IActionResult> DeleteOrderDetail(int OrderDetailId)
        {
            try
            {
                var listProducts = await _mediator.Send(new DeleteOrderDetailCommands(OrderDetailId));
                return Ok(listProducts);
            }
            catch (Exception)
            {
                return BadRequest();

            }

        }
    }
}

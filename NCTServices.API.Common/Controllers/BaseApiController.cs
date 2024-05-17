using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NCTServices.API.Common.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public abstract class BaseApiController<T> : ControllerBase
    {
        private IMediator? _mediatorInstance;

        protected IMediator _mediator => _mediatorInstance ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.OrderUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class OrdersController : RootController
    {
        private readonly IMediator _mediator;

        public OrdersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ServiceResponse<OrderResponse>), StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(RoleTypes.Admin))]
        [HttpGet(Routes.GET_ORDER)]
        public async Task<IActionResult> ViewOrderAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewOrderQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<OrderResponse>), StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpGet(Routes.GET_ORDER_BY_TRACKING_NUMBER)]
        public async Task<IActionResult> ViewOrderByTrackingNumberAsync(string trackingNumber)
            => ResolveActionResult(await _mediator.Send(new ViewOrderByTrackingNumberQuery(trackingNumber)));

        [ProducesResponseType(typeof(ServiceResponse<IList<OrderResponse>>), StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(RoleTypes.Admin))]
        [HttpGet(Routes.GET_ORDERS)]
        public async Task<IActionResult> ViewOrdersAsync([FromQuery] FilterOrdersParam request)
            => ResolveActionResult(await _mediator.Send(new ViewOrdersQuery(request)));

        [ProducesResponseType(typeof(ServiceResponse<ProductResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpPost(Routes.CREATE_ORDER)]
        public async Task<IActionResult> CreateOrderAsync([FromBody] OrderRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateOrderCommand(request, User)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin))]
        [HttpDelete(Routes.DELETE_ORDER)]
        public async Task<IActionResult> DeleteOrderAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new DeleteOrderCommand(id)));
    }
}

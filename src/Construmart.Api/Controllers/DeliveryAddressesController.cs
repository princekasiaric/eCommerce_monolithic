using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.DeliveryAddressUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class DeliveryAddressesController : RootController
    {
        private readonly IMediator _mediator;

        public DeliveryAddressesController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [ProducesResponseType(typeof(ServiceResponse<DeliveryAddressResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpPost(Routes.CREATE_DELIVERY_ADDRESS)]
        public async Task<IActionResult> CreateDeliveryAddressAsync([FromBody]DeliveryAddressRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateDeliveryAddressCommand(request, User)));

        [ProducesResponseType(typeof(ServiceResponse<IList<DeliveryAddressResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_DELIVERY_ADDRESSES)]
        public async Task<IActionResult> ViewDeliveryAddressesAsync()
            => ResolveActionResult(await _mediator.Send(new ViewDeliveryAddressesQuery()));

        [ProducesResponseType(typeof(ServiceResponse<DeliveryAddressResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_DELIVERY_ADDRESS)]
        public async Task<IActionResult> ViewDeliveryAddressAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewDeliveryAddressQuery(id)));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpPut(Routes.UPDATE_DELIVERY_ADDRESS)]
        public async Task<IActionResult> UpdateDeliveryAddressAsync([FromBody]DeliveryAddressRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateDeliveryAddressCommand(id, request, User)));

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = nameof(RoleTypes.Customer))]
        [HttpDelete(Routes.DELETE_DELIVERY_ADDRESS)]
        public async Task<IActionResult> DeleteDeliveryAddressAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new DeleteDeliveryAddressCommand(id, User)));
    }
}

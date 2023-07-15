using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.DiscountUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class DiscountsController : RootController
    {
        private readonly IMediator _mediator;

        public DiscountsController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [ProducesResponseType(typeof(ServiceResponse<DiscountResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.CREATE_DISCOUNT)]
        public async Task<IActionResult> CreateDiscountAsync([FromBody] DiscountRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateDiscountCommand(request, User)));

        [ProducesResponseType(typeof(ServiceResponse<IList<DiscountResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_DISCOUNTS)]
        public async Task<IActionResult> ViewDiscountsAsync()
            => ResolveActionResult(await _mediator.Send(new ViewDiscountsQuery()));

        [ProducesResponseType(typeof(ServiceResponse<DiscountResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_DISCOUNT)]
        public async Task<IActionResult> ViewDiscountAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewDiscountQuery(id)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPut(Routes.UPDATE_DISCOUNT)]
        public async Task<IActionResult> UpdateDiscountAsync([FromBody] DiscountRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateDiscountCommand(id, request, User)));
    }
}

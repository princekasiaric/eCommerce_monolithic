using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.BrandUseCases;
using Construmart.Core.UseCases.CategoryUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class BrandsController : RootController
    {
        private readonly IMediator _mediator;

        public BrandsController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [ProducesResponseType(typeof(ServiceResponse<BrandResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.CREATE_BRAND)]
        public async Task<IActionResult> CreateBrandAsync([FromBody] BrandRequest request)
        {
            var result = await _mediator.Send(new CreateBrandCommand(request, User));
            if (result.IsSuccess)
            {
                var newResult = result as ServiceResponse<BrandResponse>;
                return ResolveActionResult(result, "ViewBrand", newResult.Payload.Id);
            }
            return ResolveActionResult(result);
        }

        [ProducesResponseType(typeof(ServiceResponse<BrandResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_BRAND)]
        public async Task<IActionResult> ViewBrandAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewBrandQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<IList<BrandResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_BRANDS)]
        public async Task<IActionResult> ViewBrandsAsync()
            => ResolveActionResult(await _mediator.Send(new ViewBrandsQuery()));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPut(Routes.UPDATE_BRAND)]
        public async Task<IActionResult> UpdateBrandAsync([FromBody] BrandRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateBrandCommand(id, request, User)));
    }
}
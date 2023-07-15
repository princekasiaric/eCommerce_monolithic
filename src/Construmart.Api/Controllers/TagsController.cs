using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons.Utils;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.BrandUseCases;
using Construmart.Core.UseCases.CategoryUseCases;
using Construmart.Core.UseCases.ProductUseCases;
using Construmart.Core.UseCases.TagUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class TagsController : RootController
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [ProducesResponseType(typeof(ServiceResponse<TagResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.CREATE_TAG)]
        public async Task<IActionResult> CreateTagAsync([FromBody] TagRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateTagCommand(request, User)));

        [ProducesResponseType(typeof(ServiceResponse<IList<TagResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_TAGS)]
        public async Task<IActionResult> ViewTagsAsync()
            => ResolveActionResult(await _mediator.Send(new ViewTagsQuery()));

        [ProducesResponseType(typeof(ServiceResponse<TagResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_TAG)]
        public async Task<IActionResult> ViewTagAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewTagQuery(id)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPut(Routes.UPDATE_TAG)]
        public async Task<IActionResult> UpdateTagAsync([FromBody] TagRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateTagCommand(id, request, User)));

        [ProducesResponseType(typeof(ServiceResponse<ProductResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_PRODUCTS_BY_TAG_ID)]
        public async Task<IActionResult> ViewProductsByTagIdAsync(uint id, [FromQuery]TagParam queryString)
            => ResolveActionResult(await _mediator.Send(new ViewProductsByTagIdQuery(id, queryString)));
    }
}
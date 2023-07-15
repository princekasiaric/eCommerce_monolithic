using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons.Utils;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.CategoryUseCases;
using Construmart.Core.UseCases.CustomerUseCases;
using Construmart.Core.UseCases.ProductUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class CategoriesController : RootController
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = Guard.Against.Null(mediator, nameof(mediator));
        }

        [ProducesResponseType(typeof(ServiceResponse<CategoryResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.CREATE_CATEGORY)]
        public async Task<IActionResult> CreateCategoryAsync([FromBody] CategoryRequest request)
        {
            var result = await _mediator.Send(new CreateCategoryCommand(request, User));
            if (result.IsSuccess)
            {
                var newResult = result as ServiceResponse<CategoryResponse>;
                return ResolveActionResult(result, "ViewCategory", newResult.Payload.Id);
            }
            return ResolveActionResult(result);
        }

        [ProducesResponseType(typeof(ServiceResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_CATEGORY)]
        public async Task<IActionResult> ViewCategoryAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewCategoryQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<IList<CategoryResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_CATEGORIES)]
        public async Task<IActionResult> ViewCategoriesAsync([FromQuery] FilterCategoriesParam query)
            => ResolveActionResult(await _mediator.Send(new ViewCategoriesQuery(query)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPut(Routes.UPDATE_CATEGORY)]
        public async Task<IActionResult> UpdateCategoryAsync([FromBody] CategoryRequest request, uint id)
            => ResolveActionResult(await _mediator.Send(new UpdateCategoryCommand(id, request, User)));

        [ProducesResponseType(typeof(ServiceResponse<ProductResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_PRODUCTS_BY_CATEGORY_ID)]
        public async Task<IActionResult> ViewProductsByCategoryIdAsync(uint id, [FromQuery]CategoryParam queryString)
            => ResolveActionResult(await _mediator.Send(new ViewProductsByCategoryIdQuery(id, queryString)));
    }
}
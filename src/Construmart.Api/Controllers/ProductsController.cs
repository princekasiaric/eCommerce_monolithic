using System.Collections.Generic;
using System.Threading.Tasks;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.ProductUseCases;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class ProductsController : RootController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ServiceResponse<ProductResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_PRODUCT)]
        public async Task<IActionResult> ViewProductAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewProductQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<IList<ProductResponse>>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_PRODUCTS)]
        public async Task<IActionResult> ViewProductsAsync([FromQuery] FilterProductsParam request)
            => ResolveActionResult(await _mediator.Send(new ViewProductsQuery(request)));

        [ProducesResponseType(typeof(ServiceResponse<ProductResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.CREATE_PRODUCT)]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateProductCommand(request, User)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPut(Routes.UPDATE_PRODUCT)]
        public async Task<IActionResult> UpdateProductAsync([FromBody] ProductRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateProductCommand(id, request, User)));

        [ProducesResponseType(typeof(ServiceResponse<UploadProductImageResponse>), StatusCodes.Status201Created)]
        [Authorize(Roles = nameof(RoleTypes.Admin) + "," + nameof(RoleTypes.SuperAdmin))]
        [HttpPost(Routes.PRODUCT_IMAGE_UPLOAD)]
        public async Task<IActionResult> UploadProductImageAsync(uint id, [FromBody] ImageUploadRequest request)
            => ResolveActionResult(await _mediator.Send(new UploadProductImageCommand(id, request, User)));
    }
}

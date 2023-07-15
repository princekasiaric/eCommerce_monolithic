using System.Threading.Tasks;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.UseCases.CartUseCases;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Construmart.Api.Controllers
{
    [ApiController]
    [Route(Routes.ROOT)]
    public class CartsController : RootController
    {
        private readonly IMediator _mediator;

        public CartsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ServiceResponse<CartResponse>), StatusCodes.Status200OK)]
        [HttpGet(Routes.GET_CART)]
        public async Task<IActionResult> ViewCartAsync(uint id)
            => ResolveActionResult(await _mediator.Send(new ViewCartQuery(id)));

        [ProducesResponseType(typeof(ServiceResponse<CartResponse>), StatusCodes.Status201Created)]
        [HttpPost(Routes.CREATE_CART)]
        public async Task<IActionResult> CreateCartAsync([FromBody]CartRequest request)
            => ResolveActionResult(await _mediator.Send(new CreateCartCommand(request)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpPut(Routes.UPDATE_CART)]
        public async Task<IActionResult> UpdateCartAsync([FromBody]CartRequest request, int id)
            => ResolveActionResult(await _mediator.Send(new UpdateCartCommand(id, request)));

        [ProducesResponseType(typeof(ServiceResponse<CartItemResponse>), StatusCodes.Status204NoContent)] 
        [HttpGet(Routes.GET_CARTITEM)]
        public async Task<IActionResult> ViewCartItemAsync(int cartId, int productId)
            => ResolveActionResult(await _mediator.Send(new ViewCartItemQuery(cartId, productId)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete(Routes.CLEAR_CART)]
        public async Task<IActionResult> ClearCartAsync(int id)
            => ResolveActionResult(await _mediator.Send(new ClearCartCommand(id)));

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [HttpDelete(Routes.CLEAR_CARTITEM)]
        public async Task<IActionResult> ClearCartItemAsync(int cartId, int cartItemId)
            => ResolveActionResult(await _mediator.Send(new ClearCartItemCommand(cartId, cartItemId)));
    }
}

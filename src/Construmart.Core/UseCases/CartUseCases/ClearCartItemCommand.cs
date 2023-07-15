using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CartUseCases
{
    public class ClearCartItemCommand : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }
        public long CartItemId { get; set; }

        public ClearCartItemCommand(long cartId, long cartItemId)
        {
            CartId = cartId;
            CartItemId = cartItemId;
        }
    }

    public class ClearcartItemCommandHandler : IRequestHandler<ClearCartItemCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public ClearcartItemCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(ClearCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repositoryManager.CartRepo.SingleOrDefaultAsync(
                x => x.Id == request.CartId,
                includes: new System.Linq.Expressions.Expression<Func<Domain.Models.Cart, object>>[] { x => x.CartItems });

            if (cart == null)
            {
                return _result.Failure(ResponseCodes.InvalidCart, StatusCodes.Status404NotFound);
            }

            if (!cart.CartItems.Any())
            {
                return _result.Failure(ResponseCodes.EmptyCart);
            }

            var cartItem = cart.CartItems.SingleOrDefault(x => x.Id == request.CartItemId);

            if (cartItem == null)
            {
                return _result.Failure(ResponseCodes.InvalidCartItem, StatusCodes.Status404NotFound);
            }

            cart.RemoveCartItem(cartItem);
            _repositoryManager.CartRepo.Update(cart);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}

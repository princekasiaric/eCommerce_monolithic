using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CartUseCases
{
    public class ViewCartItemQuery : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }
        public long ProductId { get; private set; }

        public ViewCartItemQuery(long cartId, long productId)
        {
            CartId = cartId;
            ProductId = productId;
        }
    }

    public class ViewCartItemQueryHandler : IRequestHandler<ViewCartItemQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewCartItemQueryHandler(
            IResult result,
            IMapper mapper,
            IRepositoryManager repositoryManager)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(ViewCartItemQuery request, CancellationToken cancellationToken)
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

            var productExist = await _repositoryManager.ProductRepo.AnyAsync(x => x.Id == request.ProductId);
            if (!productExist)
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }

            var cartItem = cart.CartItems.SingleOrDefault(x => x.ProductId == request.ProductId);
            var cartItemResponse = _mapper.Map<CartItemResponse>(cartItem);
            return _result.Success(cartItemResponse);
        }
    }
}

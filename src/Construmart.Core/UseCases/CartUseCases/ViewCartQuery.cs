using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CartUseCases
{
    public class ViewCartQuery : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }

        public ViewCartQuery(long cartId)
        {
            CartId = cartId;
        }
    }

    public class ViewCartQueryHandler : IRequestHandler<ViewCartQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public ViewCartQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _repositoryManager.CartRepo.SingleOrDefaultAsync(x => x.Id == request.CartId,
                includes: new Expression<Func<Cart, object>>[] { x => x.CartItems });
            if (cart == null)
            {
                return _result.Failure(ResponseCodes.InvalidCart, StatusCodes.Status404NotFound);
            }
            var cartItems = new List<CartItemResponse>();
            foreach (var cartItem in cart.CartItems)
            {
                var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(x => x.Id == cartItem.ProductId);
                var discount = await _repositoryManager.DiscountRepo.SingleOrDefaultAsync(x => x.Id == product.DiscountId);
                var percentageOff = (discount != null) ? (discount.PercentageOff * 0.01) : 0;
                var discountedPrice = product.UnitPrice * Convert.ToDecimal(percentageOff);
                if (product != null)
                {
                    cartItems.Add(new CartItemResponse
                    {
                        Id = cartItem.Id,
                        ProductId = cartItem.ProductId,
                        ProductName = product.Name,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Quantity * (product.UnitPrice - discountedPrice)
                    });
                }
                else
                {
                    return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
                }
            }
            var totalPrice = cartItems.Sum(x => x.Price);
            return _result.Success(new CartResponse
            {
                Id = cart.Id,
                HasCheckout = cart.HasCheckout,
                CartItems = cartItems,
                TotalPrice = totalPrice,
                DateCreated = cart.DateCreated,
                DateUpdated = cart.DateUpdated,
                CreatedByUserId = cart.CreatedByUserId,
                UpdatedByUserId = cart.UpdatedByUserId
            });
        }
    }
}

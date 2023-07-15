using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CartUseCases
{
    public class CreateCartCommand : RequestContext<BaseResponse>
    {
        public long ProductId { get; private set; }
        public int Quantity { get; private set; }

        public CreateCartCommand(CartRequest request)
        {
            ProductId = request.ProductId;
            Quantity = request.Quantity;
        }
    }

    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }

    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public CreateCartCommandHandler(
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

        public async Task<BaseResponse> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(x => x.Id == request.ProductId);

            if (product == null)
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }

            var cart = Cart.Create(request.ProductId, request.Quantity);
            await _repositoryManager.CartRepo.AddAsync(cart);
            await _repositoryManager.SaveAsync();

            if (cart.Id <= 0)
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);

            var cartItems = cart.CartItems.Select(x => new CartItemResponse
            {
                Id = x.Id,
                ProductId = x.ProductId,
                ProductName = product.Name,
                Quantity = x.Quantity,
                Price = x.Quantity * product.UnitPrice
            }).ToList();
            var totalPrice = cartItems.Select(x => x.Price).Sum();
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
            }, StatusCodes.Status201Created);
        }
    }
}

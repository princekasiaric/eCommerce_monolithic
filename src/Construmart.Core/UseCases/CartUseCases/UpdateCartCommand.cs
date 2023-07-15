using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
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
    public class UpdateCartCommand : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }
        public long ProductId { get; private set; }
        public int Quantity { get; private set; }

        public UpdateCartCommand(long cartId, CartRequest request)
        {
            CartId = cartId;
            ProductId = request.ProductId;
            Quantity = request.Quantity;
        }
    }

    public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }

    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public UpdateCartCommandHandler(
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

        public async Task<BaseResponse> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(x => x.Id == request.ProductId);
            if (product == null)
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }
            var cart = await _repositoryManager.CartRepo.SingleOrDefaultAsync(x => x.Id == request.CartId,
               includes: new Expression<Func<Cart, object>>[] { x => x.CartItems });
            if (cart == null)
            {
                return _result.Failure(ResponseCodes.InvalidCart, StatusCodes.Status404NotFound);
            }
            cart.Update(request.ProductId, request.Quantity);
            _repositoryManager.CartRepo.Update(cart);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}

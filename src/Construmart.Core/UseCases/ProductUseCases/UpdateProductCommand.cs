using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.ProductUseCases
{
    public class UpdateProductCommand : RequestContext<BaseResponse>
    {
        public long Id { get; private set; }
        public long? BrandId { get; private set; }
        public long? DiscountId { get; private set; }
        public IList<long> CategoryIds { get; private set; }
        public IList<long> TagIds { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public bool IsActive { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public UpdateProductCommand(long id, ProductRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Id = id;
            BrandId = request.BrandId;
            DiscountId = request.DiscountId;
            CategoryIds = request.CategoryIds;
            TagIds = request.TagIds;
            Name = HttpUtility.HtmlEncode(request.Name);
            Description = request.Description;
            UnitPrice = request.UnitPrice;
            IsActive = request.IsActive;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            When(x => x.DiscountId.HasValue, () =>
            {
                RuleFor(x => x.DiscountId).NotNull().GreaterThan(0);
            });
            When(x => !x.DiscountId.HasValue, () =>
            {
                RuleFor(x => x.DiscountId).Null();
            });
            RuleFor(x => x.TagIds).NotEmpty();
            RuleFor(x => x.CategoryIds).NotEmpty();
            RuleFor(x => x.BrandId).NotNull().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Quantity).NotEqual(0);
            RuleFor(x => x.UnitPrice).NotNull().GreaterThan(0);
        }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;

        public UpdateProductCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager,
            IIdentityService identityService)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var brandExists = await _repositoryManager.BrandRepo.AnyAsync(x => x.Id == request.BrandId);
            if (!brandExists)
                return _result.Failure(ResponseCodes.InvalidBrand, StatusCodes.Status404NotFound);
            var discountExists = await _repositoryManager.DiscountRepo.AnyAsync(x => x.Id == request.DiscountId);
            if (!discountExists)
                return _result.Failure(ResponseCodes.InvalidDiscount, StatusCodes.Status404NotFound);
            var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(
                x => x.Id == request.Id,
                includes: new Expression<Func<Product, object>>[] { x => x.ProductInventories },
                withTracking: true);
            if (product == null)
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);

            foreach (var id in request.CategoryIds)
            {
                var categoryExists = await _repositoryManager.CategoryRepo.AnyAsync(x => x.Id == id);
                if (!categoryExists)
                    return _result.Failure(ResponseCodes.InvalidCategory, StatusCodes.Status404NotFound);
            }
            foreach (var id in request.TagIds)
            { 
                var tagExists = await _repositoryManager.TagRepo.AnyAsync(x => x.Id == id);
                if (!tagExists)
                    return _result.Failure(ResponseCodes.InvalidTag, StatusCodes.Status404NotFound);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var productInventory = await _repositoryManager.ProductInventoryRepo.LastOrDefaultAsync(x => x.ProductId == product.Id);
            product.Update(
                request.BrandId,
                request.DiscountId,
                request.CategoryIds,
                request.TagIds,
                userIdResult.Payload.ApplicationUserId,
                request.Name,
                request.Description,
                request.UnitPrice,
                EnumerationBase.FromDisplayName<CurrencyCodes>(CurrencyCodes.NGN.DisplayName),
                request.IsActive);
            var newTotalStock = productInventory.NewTotalStock + request.Quantity;
            var newTotalPrice = productInventory.NewTotalStock * request.UnitPrice;
            product.UpdateInventory(userIdResult.Payload.ApplicationUserId, request.Quantity, request.UnitPrice);
            _repositoryManager.ProductRepo.Update(product);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}

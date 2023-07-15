using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
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
    public class CreateProductCommand : RequestContext<BaseResponse>
    {
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

        public CreateProductCommand(ProductRequest request, ClaimsPrincipal claimsPrincipal)
        {
            BrandId = request.BrandId;
            DiscountId = request.DiscountId;
            CategoryIds = request.CategoryIds;
            TagIds = request.TagIds;
            Name = request.Name;
            Description = request.Description;
            Quantity = request.Quantity;
            UnitPrice = request.UnitPrice;
            IsActive = request.IsActive;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            When(x => x.DiscountId.HasValue, () =>
              {
                  RuleFor(x => x.DiscountId).NotNull().GreaterThan(0);
              });
            When(x => !x.DiscountId.HasValue, () =>
            {
                RuleFor(x => x.DiscountId).Null();
            });
            RuleFor(x => x.BrandId).NotNull().GreaterThan(0);
            RuleFor(x => x.CategoryIds).NotEmpty();
            RuleFor(x => x.TagIds).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Quantity).NotEqual(0);
            RuleFor(x => x.UnitPrice).NotNull().GreaterThan(0);
        }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager,
            IIdentityService identityService,
            IMapper mapper)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var brandExists = await _repositoryManager.BrandRepo.AnyAsync(x => x.Id == request.BrandId);
            if (!brandExists)
                return _result.Failure(ResponseCodes.InvalidBrand, StatusCodes.Status404NotFound);
            var discountExists = false;
            if (request.DiscountId != null)
            {
                discountExists = await _repositoryManager.DiscountRepo.AnyAsync(x => x.Id == request.DiscountId);
                if (!discountExists)
                    return _result.Failure(ResponseCodes.InvalidDiscount, StatusCodes.Status404NotFound);
            }
            var productExists = await _repositoryManager.ProductRepo.AnyAsync(x => x.Name.Trim().Replace(" ", string.Empty).ToLower() == request.Name.Trim().Replace(" ", string.Empty).ToLower());
            if (productExists)
                return _result.Failure(ResponseCodes.DuplicateProduct, StatusCodes.Status404NotFound);

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
            var sku = request.Name.Trim().Replace(" ", string.Empty)[..4] + Guid.NewGuid().ToString("N");
            var product = Product.Create(
                request.BrandId,
                request.DiscountId,
                request.CategoryIds,
                request.TagIds,
                userIdResult.Payload.ApplicationUserId,
                sku: sku,
                request.Name,
                request.Description,
                request.UnitPrice,
                EnumerationBase.FromDisplayName<CurrencyCodes>(CurrencyCodes.NGN.DisplayName), 
                request.IsActive);
            product.UpdateInventory(userIdResult.Payload.ApplicationUserId, request.Quantity, request.UnitPrice);
            await _repositoryManager.ProductRepo.AddAsync(product);
            await _repositoryManager.SaveAsync();
            if (product.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }

            var productCategories = new List<ProductCategoryResponse>();
            foreach (var id in product.ProductCategoryIds)
            {
                var category = await _repositoryManager.CategoryRepo.SingleOrDefaultAsync(x => x.Id == id);
                if (category != null)
                {
                    productCategories.Add(new ProductCategoryResponse
                    {
                        Id = category.Id,
                        Name = category.Name
                    });
                }
                else
                {
                    return _result.Failure(ResponseCodes.InvalidCategory, StatusCodes.Status404NotFound);
                }
            }
            var productTags = new List<ProductTagResponse>();
            foreach (var id in product.ProductTagIds)
            {
                var tag = await _repositoryManager.TagRepo.SingleOrDefaultAsync(x => x.Id == id);
                if (tag != null)
                {
                    productTags.Add(new ProductTagResponse
                    {
                        Id = tag.Id,
                        Name = tag.Name
                    });
                }
                else
                {
                    return _result.Failure(ResponseCodes.InvalidTag, StatusCodes.Status404NotFound);
                }
            }
            var createdProductResponse = _mapper.Map<ProductResponse>(product);
            createdProductResponse.ProductCategories = productCategories;
            createdProductResponse.ProductTags = productTags;
            return _result.Success(createdProductResponse, StatusCodes.Status201Created);
        }
    }
}

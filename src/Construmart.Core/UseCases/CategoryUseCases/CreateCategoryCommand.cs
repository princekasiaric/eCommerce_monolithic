using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CategoryUseCases
{
    public class CreateCategoryCommand : RequestContext<BaseResponse>
    {
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsParent { get; private set; }
        public long? ParentCategoryId { get; private set; }
        // public IFormFile ImageFile { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public CreateCategoryCommand(CategoryRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Name = HttpUtility.HtmlEncode(request.Name);
            IsActive = request.IsActive;
            IsParent = request.IsParent;
            ParentCategoryId = request.ParentCategoryId;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();

            When(x => !x.IsParent, () =>
            {
                RuleFor(x => x.ParentCategoryId).NotNull().GreaterThan(0);
            });

            When(x => x.IsParent, () =>
            {
                RuleFor(x => x.ParentCategoryId).Null();
            });

            // When(x => x.ImageFile != null, () =>
            // {
            //     RuleFor(x => x.ImageFile.Name).NotNull().NotEmpty();
            //     RuleFor(x => x.ImageFile.Length).GreaterThan(0);
            //     RuleFor(x => x.ImageFile.FileName)
            //         .NotNull()
            //         .NotEmpty()
            //         .Custom((x, ctx) =>
            //         {
            //             if (!string.IsNullOrWhiteSpace(x))
            //             {
            //                 var extension = Path.GetExtension(x);
            //                 if (!string.IsNullOrWhiteSpace(extension))
            //                 {
            //                     ctx.AddFailure("Invalid image file. File must have '.jpg', '.jpeg', or '.png' extension.");
            //                 }
            //                 else
            //                 {
            //                     var isValidExtension = extension.ToLower() switch
            //                     {
            //                         ".jpg" => true,
            //                         ".jpeg" => true,
            //                         ".png" => true,
            //                         _ => false
            //                     };
            //                     if (!isValidExtension)
            //                     {
            //                         ctx.AddFailure("Invalid image file. File must have '.jpg', '.jpeg', or '.png' extension.");
            //                     }
            //                 }
            //             }
            //         });
            // });
        }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateCategoryCommandHandler(
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

        public async Task<BaseResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request.ParentCategoryId.HasValue)
            {
                var parentCategoryExists = await _repositoryManager.CategoryRepo.AnyAsync(x => x.Id == request.ParentCategoryId && x.IsParent);
                if (!parentCategoryExists)
                {
                    return _result.Failure(ResponseCodes.InvalidParentCategory);
                }
            }
            var categoryExists = await _repositoryManager.CategoryRepo.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (categoryExists)
            {
                return _result.Failure(ResponseCodes.DuplicateCategory);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var category = Category.Create(
                request.Name,
                request.IsActive,
                request.IsParent,
                request.ParentCategoryId,
                userIdResult.Payload.ApplicationUserId);
            await _repositoryManager.CategoryRepo.AddAsync(category);
            await _repositoryManager.SaveAsync();
            if (category.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var createdCategoryResponse = _mapper.Map<CategoryResponse>(category);
            return _result.Success(createdCategoryResponse, StatusCodes.Status201Created);
        }
    }
}
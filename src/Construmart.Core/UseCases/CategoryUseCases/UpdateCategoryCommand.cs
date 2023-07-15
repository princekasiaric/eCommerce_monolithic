using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CategoryUseCases
{
    public class UpdateCategoryCommand : RequestContext<BaseResponse>
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsParent { get; private set; }
        public long? ParentCategoryId { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public UpdateCategoryCommand(long id, CategoryRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Id = id;
            Name = HttpUtility.HtmlEncode(request.Name);
            IsActive = request.IsActive;
            IsParent = request.IsParent;
            ParentCategoryId = request.ParentCategoryId;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
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
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;

        public UpdateCategoryCommandHandler(IResult result, IRepositoryManager repositoryManager, IIdentityService identityService)
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

        public async Task<BaseResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _repositoryManager.CategoryRepo.SingleOrDefaultAsync(x => x.Id == request.Id, withTracking: true);
            if (category == null)
            {
                return _result.Failure(ResponseCodes.InvalidCategory, StatusCodes.Status404NotFound);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            category.Update(
                request.Name,
                request.IsActive,
                request.IsParent,
                request.ParentCategoryId,
                userIdResult.Payload.ApplicationUserId);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}
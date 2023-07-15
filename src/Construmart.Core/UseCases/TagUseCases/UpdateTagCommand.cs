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

namespace Construmart.Core.UseCases.TagUseCases
{
    public class UpdateTagCommand : RequestContext<BaseResponse>
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public UpdateTagCommand(long id, TagRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Id = id;
            Name = HttpUtility.HtmlEncode(request.Name);
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateTagCommand, BaseResponse>, IDisposable
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

        public async Task<BaseResponse> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repositoryManager.TagRepo.SingleOrDefaultAsync(x => x.Id == request.Id, withTracking: true);
            if (tag == null)
            {
                return _result.Failure(ResponseCodes.InvalidTag, StatusCodes.Status404NotFound);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            tag.Update(request.Name, userIdResult.Payload.ApplicationUserId);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}
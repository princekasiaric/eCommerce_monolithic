using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using Construmart.Core.UseCases.BrandUseCases;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.TagUseCases
{
    public class CreateTagCommand : RequestContext<BaseResponse>
    {
        public string Name { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public CreateTagCommand(TagRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Name = request.Name;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateTagCommandHandler(
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

        public async Task<BaseResponse> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tagExists = await _repositoryManager.TagRepo.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (tagExists)
            {
                return _result.Failure(ResponseCodes.DuplicateTag);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var tag = Tag.Create(request.Name, userIdResult.Payload.ApplicationUserId);

            await _repositoryManager.TagRepo.AddAsync(tag);
            await _repositoryManager.SaveAsync();
            if (tag.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var createdTagResponse = _mapper.Map<TagResponse>(tag);
            return _result.Success(createdTagResponse, StatusCodes.Status201Created);
        }
    }
}
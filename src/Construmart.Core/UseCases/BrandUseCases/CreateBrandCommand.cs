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
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.BrandUseCases
{
    public class CreateBrandCommand : RequestContext<BaseResponse>
    {
        public string Name { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public CreateBrandCommand(BrandRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Name = request.Name;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateBrandCommandValidator : AbstractValidator<CreateBrandCommand>
    {
        public CreateBrandCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

    public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateBrandCommandHandler(
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

        public async Task<BaseResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var brandExists = await _repositoryManager.BrandRepo.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (brandExists)
            {
                return _result.Failure(ResponseCodes.DuplicateBrand);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var brand = Brand.Create(request.Name, userIdResult.Payload.ApplicationUserId);

            await _repositoryManager.BrandRepo.AddAsync(brand);
            await _repositoryManager.SaveAsync();
            if (brand.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var createdBrandResponse = _mapper.Map<BrandResponse>(brand);
            return _result.Success(createdBrandResponse, StatusCodes.Status201Created);
        }
    }
}
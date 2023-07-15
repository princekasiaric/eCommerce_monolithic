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

namespace Construmart.Core.UseCases.DiscountUseCases
{
    public class CreateDiscountCommand : RequestContext<BaseResponse>
    {
        public string Name { get; private set; }
        public double PercentageOff { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; }

        public CreateDiscountCommand(DiscountRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Name = request.Name;
            PercentageOff = request.PercentageOff;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateDiscountCommandValidator : AbstractValidator<CreateDiscountCommand>
    {
        public CreateDiscountCommandValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }

    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateDiscountCommandHandler(
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

        public async Task<BaseResponse> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var discountExists = await _repositoryManager.DiscountRepo.AnyAsync(x => x.Name.ToLower() == request.Name.ToLower());
            if (discountExists)
            {
                return _result.Failure(ResponseCodes.DuplicateDiscount);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var discount = Discount.Create(request.Name, request.PercentageOff, userIdResult.Payload.ApplicationUserId);
            await _repositoryManager.DiscountRepo.AddAsync(discount);
            await _repositoryManager.SaveAsync();
            if (discount.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var createdDiscountResponse = _mapper.Map<DiscountResponse>(discount);
            return _result.Success(createdDiscountResponse, StatusCodes.Status201Created);
        }
    }
}
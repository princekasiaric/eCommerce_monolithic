using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.ValueObjects;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CustomerUseCases
{
    public class UpdateCustomerDetailsCommand : RequestContext<BaseResponse>
    {
        public UpdateCustomerDetailsCommand(UpdateCustomerDetailsRequest request)
        {
            PhoneNumber = request.PhoneNumber;
            StreetNumber = request.StreetNumber;
            StreetName = request.StreetName;
            State = request.State;
            ZipCode = request.ZipCode;
        }
        public string PhoneNumber { get; }
        public string StreetNumber { get; }
        public string StreetName { get; }
        public string State { get; }
        public string ZipCode { get; }
    }

    public class UpdateCustomerDetailsCommandValidator : AbstractValidator<UpdateCustomerDetailsCommand>
    {
        public UpdateCustomerDetailsCommandValidator()
        {
            RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .NotNull()
            .Matches(new Regex(Constants.AppRegex.PHONE_NUMBER, RegexOptions.Compiled));

            RuleFor(x => x.StreetNumber)
            .NotEmpty()
            .NotNull()
            .Matches(new Regex(Constants.AppRegex.ALPHANUMERIC, RegexOptions.Compiled));

            RuleFor(x => x.StreetName)
            .NotEmpty()
            .NotNull()
            .Matches(new Regex(Constants.AppRegex.ALPHABET, RegexOptions.Compiled));

            RuleFor(x => x.State)
            .NotEmpty()
            .NotNull()
            .Matches(new Regex(Constants.AppRegex.ALPHABET, RegexOptions.Compiled));

            RuleFor(x => x.ZipCode)
            .NotEmpty()
            .NotNull()
            .Matches(new Regex(Constants.AppRegex.DIGIT, RegexOptions.Compiled));
        }
    }

    public class UpdateCustomerDetailsCommandHandler : IRequestHandler<UpdateCustomerDetailsCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIdentityService _identityService;

        public UpdateCustomerDetailsCommandHandler(
            IHttpContextAccessor httpContextAccessor,
            IRepositoryManager repositoryManager,
            IIdentityService identityService,
            IResult result)
        {
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _result = Guard.Against.Null(result, nameof(result));
            _httpContextAccessor = Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(UpdateCustomerDetailsCommand request, CancellationToken cancellationToken)
        {
            var userResult = _identityService.GetUserIdFromClaims(_httpContextAccessor.HttpContext.User);
            if (!userResult.IsSuccess)
                return userResult;
            var newUserResult = userResult as ServiceResponse<UserIdResponse>;
            var customer = await _repositoryManager.CustomerRepo.SingleOrDefaultAsync(x => x.ApplicationUserId.Equals(newUserResult.Payload.ApplicationUserId), withTracking: true);
            if (customer == null)
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            await _repositoryManager.BeginTransactionAsync();
            customer.Update(request.PhoneNumber, request.StreetName, request.StreetNumber, request.State, request.ZipCode);
            await _identityService.UpdatePhoneNumberAsync(customer.ApplicationUserId, request.PhoneNumber);
            await _repositoryManager.SaveAsync();
            await _repositoryManager.CommitAsync();
            return _result.Success();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CustomerUseCases
{
    public class CompleteCustomerSignupCommand : RequestContext<BaseResponse>
    {
        public CompleteCustomerSignupCommand(CompleteCustomerSignupRequest request)
        {
            Email = request.Email;
            Otp = request.Otp;
        }

        public string Email { get; }
        public string Otp { get; }
    }

    public class CompleteCustomerSignupCommandValidator : AbstractValidator<CompleteCustomerSignupCommand>
    {
        public CompleteCustomerSignupCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .Matches(new Regex(Constants.AppRegex.EMAIL, RegexOptions.Compiled));

            RuleFor(x => x.Otp).NotNull().NotEmpty();
        }
    }

    public class CompleteCustomerSignupCommandHandler : IRequestHandler<CompleteCustomerSignupCommand, BaseResponse>, IDisposable
    {
        private readonly IIdentityService _identityService;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IResult _result;

        public CompleteCustomerSignupCommandHandler(
            IIdentityService identityService,
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IResult responseUtility)
        {
            _identityService = identityService;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _result = responseUtility;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(CompleteCustomerSignupCommand request, CancellationToken cancellationToken)
        {
            //get customer with email and onbarding status initiated
            var customer = await _repositoryManager.CustomerRepo.SingleOrDefaultAsync(
                x => x.Email == request.Email
                && x.OnboardingStatus == CustomerOnboardingStatus.Initiated,
                withTracking: true);
            if (customer == null)
            {
                return _result.Failure(ResponseCodes.EmailTaken);
            }
            var otpResult = await _identityService.VerifyOtp(customer.ApplicationUserId, request.Otp);
            if (!otpResult.IsSuccess)
            {
                return otpResult;
            }
            _repositoryManager.BeginTransaction();
            //call indentity service to activate the customer
            var activationResult = await _identityService.ActivateAsync(customer.ApplicationUserId, new List<string> { RoleTypes.Customer.DisplayName });
            if (!activationResult.IsSuccess)
            {
                return activationResult;
            }
            //call identity service to authenticate the customer
            var authResult = await _identityService.AuthenticateAsync(customer.ApplicationUserId);
            if (!authResult.IsSuccess)
            {
                return authResult;
            }

            var loginResponse = _mapper.Map<LoginResponse>((authResult as ServiceResponse<AuthenticateUserResponse>)?.Payload);
            //update customer onboarding status to completed
            customer.CompleteSignup();
            _repositoryManager.Save();
            _repositoryManager.Commit();
            return _result.Success(loginResponse, StatusCodes.Status201Created);
        }
    }
}
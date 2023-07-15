using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CustomerUseCases
{
    //command
    public class InitiateCustomerSignupCommand : RequestContext<BaseResponse>
    {
        public InitiateCustomerSignupCommand(InitiateCustomerSignupRequest request)
        {
            FirstName = HttpUtility.HtmlEncode(request.FirstName);
            LastName = HttpUtility.HtmlEncode(request.LastName);
            Email = HttpUtility.HtmlEncode(request.Email);
            Gender = request.Gender;
            Password = request.Password;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public int Gender { get; }
        public string Password { get; }
    }

    //command validator
    public class InitiateCustomerSignupCommandValidator : AbstractValidator<InitiateCustomerSignupCommand>
    {
        public InitiateCustomerSignupCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .Matches(new Regex(Constants.AppRegex.EMAIL, RegexOptions.Compiled));

            RuleFor(x => x.FirstName).NotNull().NotEmpty();

            RuleFor(x => x.LastName).NotNull().NotEmpty();

            RuleFor(x => x.Gender)
                .GreaterThan(0)
                .Custom((x, ctx) =>
                    {
                        if (x > 0 && EnumerationBase.FromValue<Gender>(x) == null)
                        {
                            ctx.AddFailure("'Gender' does not exist.");
                        }
                    });

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .Matches(new Regex(Constants.AppRegex.PASSWORD, RegexOptions.Compiled))
                .WithMessage("'Password' must contain atleast one uppercase, one lowercase, one digit, and one special character");
        }

        //
        public class InitiateCustomerSignupCommandHandler : IRequestHandler<InitiateCustomerSignupCommand, BaseResponse>, IDisposable
        {
            private readonly IIdentityService _identityService;
            private readonly IRepositoryManager _repositoryManager;
            private readonly IMapper _mapper;
            private readonly IResult _result;

            public InitiateCustomerSignupCommandHandler(
                IIdentityService identityService,
                IRepositoryManager repositoryManager,
                IMapper mapper,
                IResult result)
            {
                _identityService = Guard.Against.Null(identityService, nameof(identityService));
                _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
                _mapper = Guard.Against.Null(mapper, nameof(mapper));
                _result = Guard.Against.Null(result, nameof(result));
            }

            public void Dispose()
            {
                _repositoryManager.Dispose();
                GC.SuppressFinalize(this);
            }

            public async Task<BaseResponse> Handle(InitiateCustomerSignupCommand request, CancellationToken cancellationToken)
            {
                var customer = await _repositoryManager.CustomerRepo.SingleOrDefaultAsync(x => x.Email == request.Email, withTracking: true);
                if (customer != null && customer.OnboardingStatus.Equals(CustomerOnboardingStatus.Completed))
                {
                    return _result.Failure(ResponseCodes.EmailTaken);
                }
                await _repositoryManager.BeginTransactionAsync();
                var createUserRequest = _mapper.Map<CreateUserRequest>(request);
                var result = await _identityService.RegisterAsync(createUserRequest, Clients.CustomerApp);
                if (!result.IsSuccess)
                {
                    return result;
                }
                var userResult = result as ServiceResponse<UserIdResponse>;
                if (customer != null)
                {
                    customer.Update(request.FirstName, request.LastName, EnumerationBase.FromValue<Gender>(request.Gender));
                }
                else
                {
                    customer = Customer.Create(
                    userResult.Payload.ApplicationUserId,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    EnumerationBase.FromValue<Gender>(request.Gender));
                    await _repositoryManager.CustomerRepo.AddAsync(customer);
                    await _repositoryManager.SaveAsync();
                    if (customer.Id <= 0)
                    {
                        return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
                    }
                }
                await _identityService.SendOtp(customer.ApplicationUserId, OtpPurpose.Signup);
                _repositoryManager.Save();
                _repositoryManager.Commit();
                return _result.Success();
            }
        }
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.AuthUseCases
{
    //command
    public class LoginCommand : RequestContext<BaseResponse>
    {
        public LoginCommand(LoginRequest request, string xClient)
        {
            Email = request.Email;
            Password = request.Password;
            XClient = xClient;
        }

        public string Email { get; }
        public string Password { get; }
        public string XClient { get; }
    }

    //command validator
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotEmpty().NotEmpty();
            RuleFor(x => x.XClient)
                .NotNull()
                .NotEmpty()
                .Custom((x, ctx) =>
                    {
                        if (!string.IsNullOrWhiteSpace(x) && EnumerationBase.FromDisplayName<Clients>(x, true) == null)
                        {
                            ctx.AddFailure("'XClient' is not valid");
                        }
                    });
        }
    }

    //command handler
    public class LoginCommandHandler : IRequestHandler<LoginCommand, BaseResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _objectMapper;
        private readonly IResult _result;

        public LoginCommandHandler(
            IIdentityService identityService,
            IMapper objectMapper,
            IResult responseUtility)
        {
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _objectMapper = Guard.Against.Null(objectMapper, nameof(objectMapper));
            _result = Guard.Against.Null(responseUtility, nameof(responseUtility));
        }

        public async Task<BaseResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var client = EnumerationBase.FromDisplayName<Clients>(request.XClient, true);
            var identityRequest = _objectMapper.Map<AuthenticateUserRequest>(request);
            var authResult = await _identityService.AuthenticateAsync(identityRequest, client);
            if (!authResult.IsSuccess)
            {
                return authResult;
            }

            var loginResponse = _objectMapper.Map<LoginResponse>((authResult as ServiceResponse<AuthenticateUserResponse>)?.Payload);
            return _result.Success(loginResponse);
        }
    }
}
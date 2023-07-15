using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

namespace Construmart.Core.UseCases.AuthUseCases
{
    public class CompleteResetPasswordCommand : RequestContext<BaseResponse>
    {
        public CompleteResetPasswordCommand(CompleteResetPasswordRequest request, string xClient)
        {
            Email = request.Email;
            Otp = request.Otp;
            Password = request.Password;
            XClient = xClient;
        }

        public string Email { get; }
        public string Otp { get; }
        public string Password { get; }
        public string XClient { get; }
    }

    public class CompleteResetPasswordCommandValidator : AbstractValidator<CompleteResetPasswordCommand>
    {
        public CompleteResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Otp).NotNull().NotEmpty();
            RuleFor(x => x.XClient)
                .NotNull()
                .NotEmpty()
                .Custom((x, ctx) =>
                    {
                        if (!string.IsNullOrEmpty(x) && EnumerationBase.FromDisplayName<Clients>(x, true) == null)
                        {
                            ctx.AddFailure("'XClient' is not valid");
                        }
                    });
        }
    }

    public class CompleteResetPasswordCommandHandler : IRequestHandler<CompleteResetPasswordCommand, BaseResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _objectMapper;
        private readonly IResult _result;

        public CompleteResetPasswordCommandHandler(
            IIdentityService identityService,
            IMapper objectMapper,
            IResult result)
        {
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _objectMapper = Guard.Against.Null(objectMapper, nameof(objectMapper));
            _result = Guard.Against.Null(result, nameof(result));
        }

        public async Task<BaseResponse> Handle(CompleteResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = EnumerationBase.FromDisplayName<Clients>(request.XClient, true);
            var identityRequest = _objectMapper.Map<CompleteResetPasswordCommand, ResetPasswordRequest>(request);
            var otpResult = await _identityService.ResetPasswordAsync(identityRequest, client);
            if (!otpResult.IsSuccess)
            {
                return otpResult;
            }
            return _result.Success();
        }
    }
}
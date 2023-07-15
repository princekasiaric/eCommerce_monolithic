using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.SeedWork;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using FluentValidation;
using MediatR;

namespace Construmart.Core.UseCases.AuthUseCases
{
    public class InitiateResetPasswordCommand : RequestContext<BaseResponse>
    {
        public InitiateResetPasswordCommand(InitiateResetPaswordRequest request, string xClient)
        {
            Email = request.Email;
            XClient = xClient;
        }

        public string Email { get; }
        public string XClient { get; }
    }

    public class InitiateResetPasswordCommandValidator : AbstractValidator<InitiateResetPasswordCommand>
    {
        public InitiateResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty();
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

    public class InitiateResetPasswordCommandHandler : IRequestHandler<InitiateResetPasswordCommand, BaseResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IResult _result;

        public InitiateResetPasswordCommandHandler(
            IIdentityService identityService,
            IResult result)
        {
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _result = Guard.Against.Null(result, nameof(result));
        }

        public async Task<BaseResponse> Handle(InitiateResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var client = EnumerationBase.FromDisplayName<Clients>(request.XClient, true);
            var otpResult = await _identityService.SendOtp(request.Email, OtpPurpose.PasswordReset, client);
            if (!otpResult.IsSuccess)
            {
                return otpResult;
            }
            return _result.Success();
        }
    }
}
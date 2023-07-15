using System.Text.RegularExpressions;
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
using FluentValidation;
using MediatR;

namespace Construmart.Core.UseCases.AuthUseCases
{
    /// <summary>
    /// command
    /// </summary>
    public class ChangePasswordCommand : RequestContext<BaseResponse>
    {
        public ChangePasswordCommand(ChangePasswordRequest request, string xClient)
        {
            CurrentPassword = request.OldPassword;
            NewPassword = request.NewPassword;
            XClient = xClient;
        }

        public string CurrentPassword { get; }
        public string NewPassword { get; }
        public string XClient { get; }
    }

    /// <summary>
    /// command validator
    /// </summary>
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotNull().NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotNull()
                .NotEmpty()
                .MinimumLength(8)
                .Matches(new Regex(Constants.AppRegex.PASSWORD, RegexOptions.Compiled))
                .WithMessage("'New Password' must contain atleast one uppercase, one lowercase, one digit, and one special character");

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

    /// <summary>
    /// command handler
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, BaseResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly IMapper _objectMapper;

        public ChangePasswordCommandHandler(
            IIdentityService identityService,
            IMapper objectMapper)
        {
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
            _objectMapper = Guard.Against.Null(objectMapper, nameof(objectMapper));
        }

        public async Task<BaseResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var client = EnumerationBase.FromDisplayName<Clients>(request.XClient, true);
            var identityRequest = _objectMapper.Map<Construmart.Core.ProcessorContracts.Identity.DTOs.ChangePasswordRequest>(request);
            var changePasswordResult = await _identityService.ChangePasswordAsync(identityRequest, client);
            return changePasswordResult;
        }
    }
}
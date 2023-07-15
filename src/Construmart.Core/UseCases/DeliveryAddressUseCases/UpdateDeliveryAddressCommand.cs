using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.DeliveryAddressUseCases
{
    public class UpdateDeliveryAddressCommand : RequestContext<BaseResponse>
    {
        public long Id { get; set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string LGA { get; private set; }
        public int StateID { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public UpdateDeliveryAddressCommand(long id, DeliveryAddressRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Id = id;
            Address = request.Address;
            City = request.City;
            LGA = request.LGA;
            StateID = request.StateID;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class UpdateDeliveryAddressCommandValidator : AbstractValidator<UpdateDeliveryAddressCommand>
    {
        public UpdateDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.LGA).NotEmpty();
            RuleFor(x => x.StateID).NotEmpty();
        }
    }

    public class UpdateDeliveryAddressCommandHandler : IRequestHandler<UpdateDeliveryAddressCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;

        public UpdateDeliveryAddressCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager,
            IIdentityService identityService)
        {
            _result = result;
            _repositoryManager = repositoryManager;
            _identityService = identityService;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(UpdateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
            var deliveryAddress = await _repositoryManager.DeliveryAddressRepo.SingleOrDefaultAsync(x => x.Id == request.Id, withTracking: true);
            if (deliveryAddress == null)
            {
                return _result.Failure(ResponseCodes.InvalidDeliveryAddress, StatusCodes.Status404NotFound);
            }
            var identityResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (!identityResult.IsSuccess)
            {
                return identityResult;
            }
            var userIdResult = identityResult as ServiceResponse<UserIdResponse>;
            var customer = await _repositoryManager.CustomerRepo.SingleOrDefaultAsync(x => x.ApplicationUserId == userIdResult.Payload.ApplicationUserId);
            if (customer == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount, StatusCodes.Status404NotFound);
            }
            var nigerianState = await _repositoryManager.NigerianStateRepo.SingleOrDefaultAsync(x => x.Id == request.StateID);
            if (nigerianState == null)
            {
                return _result.Failure(ResponseCodes.InvalidNigerianState, StatusCodes.Status404NotFound);
            }
            var lgaExist = nigerianState.LocalGovernmentAreas.Any(x => x.Equals(request.LGA));
            if (!lgaExist)
            {
                return _result.Failure(ResponseCodes.InvalidLGA, StatusCodes.Status404NotFound);
            }
            deliveryAddress.Update(request.Address, request.City, request.LGA, request.StateID);
            await _repositoryManager.SaveAsync();
            return _result.Success(StatusCodes.Status200OK);
        }
    }
}

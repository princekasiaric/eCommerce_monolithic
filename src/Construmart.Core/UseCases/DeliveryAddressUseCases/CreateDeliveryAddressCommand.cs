using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.DeliveryAddressUseCases
{
    public class CreateDeliveryAddressCommand : RequestContext<BaseResponse>
    {
        public string Address { get; private set; }
        public string City { get; private set; }
        public string LGA { get; private set; }
        public int StateID { get; private set; }
        public ClaimsPrincipal ClaimsPrincipal { get; private set; }

        public CreateDeliveryAddressCommand(DeliveryAddressRequest request, ClaimsPrincipal claimsPrincipal)
        {
            Address = HttpUtility.HtmlEncode(request.Address.Trim().ToLower());
            City = HttpUtility.HtmlEncode(request.City.Trim().ToLower());
            LGA = HttpUtility.HtmlEncode(request.LGA.Trim().ToLower());
            StateID = request.StateID;
            ClaimsPrincipal = claimsPrincipal;
        }
    }

    public class CreateDeliveryAddressValidator : AbstractValidator<CreateDeliveryAddressCommand>
    {
        public CreateDeliveryAddressValidator()
        {
            RuleFor(x => x.Address).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.LGA).NotEmpty();
            RuleFor(x => x.StateID).NotEmpty();
        }
    }

    public class CreateDeliveryAddressHandler : IRequestHandler<CreateDeliveryAddressCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public CreateDeliveryAddressHandler(
            IResult result,
            IRepositoryManager repositoryManager,
            IIdentityService identityService,
            IMapper mapper)
        {
            _result = result;
            _repositoryManager = repositoryManager;
            _identityService = identityService;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(CreateDeliveryAddressCommand request, CancellationToken cancellationToken)
        {
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
            var lgaExist = nigerianState.LocalGovernmentAreas.Any(x => x.ToLower().Equals(request.LGA));
            if (!lgaExist)
            {
                return _result.Failure(ResponseCodes.InvalidLGA, StatusCodes.Status404NotFound);
            }
            var deliveryAddress = DeliveryAddress.Create(customer.Id, request.Address, request.City, request.LGA, request.StateID);
            await _repositoryManager.DeliveryAddressRepo.AddAsync(deliveryAddress);
            await _repositoryManager.SaveAsync();
            if (deliveryAddress.Id <= 0)
            {
                return _result.Failure(ResponseCodes.GeneralError, StatusCodes.Status500InternalServerError);
            }
            var deliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);
            deliveryAddressResponse.State = nigerianState.State;
            return _result.Success(deliveryAddressResponse, StatusCodes.Status201Created);
        }
    }
}

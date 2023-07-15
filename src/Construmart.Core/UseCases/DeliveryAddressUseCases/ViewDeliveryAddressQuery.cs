using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.DeliveryAddressUseCases
{
    public class ViewDeliveryAddressQuery : RequestContext<BaseResponse>
    {
        public long Id { get; private set; }

        public ViewDeliveryAddressQuery(long id)
        {
            Id = id;
        }
    }

    public class ViewDeliveryAddressQueryValidator : AbstractValidator<ViewDeliveryAddressQuery>
    {
        public ViewDeliveryAddressQueryValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }

    public class ViewDeliveryAddressQueryHandler : IRequestHandler<ViewDeliveryAddressQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public ViewDeliveryAddressQueryHandler(
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
        }

        public async Task<BaseResponse> Handle(ViewDeliveryAddressQuery request, CancellationToken cancellationToken)
        {
            var deliveryAddress = await _repositoryManager.DeliveryAddressRepo.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (deliveryAddress == null)
            {
                return _result.Failure(ResponseCodes.InvalidDeliveryAddress, StatusCodes.Status404NotFound);
            }

            var nigerianState = await _repositoryManager.NigerianStateRepo.SingleOrDefaultAsync(x => x.Id == deliveryAddress.NigerianStateId);
            var deliveryAddressResponse = _mapper.Map<DeliveryAddressResponse>(deliveryAddress);
            deliveryAddressResponse.State = nigerianState.State;
            return _result.Success(deliveryAddressResponse);
        }
    }
}

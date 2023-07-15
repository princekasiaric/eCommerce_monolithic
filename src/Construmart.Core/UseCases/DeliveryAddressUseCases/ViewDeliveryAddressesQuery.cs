using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.DeliveryAddressUseCases
{
    public class ViewDeliveryAddressesQuery : RequestContext<BaseResponse>
    {
        public ViewDeliveryAddressesQuery()
        {
        }
    }

    public class ViewDeliveryAddressesQueryHandler : IRequestHandler<ViewDeliveryAddressesQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public ViewDeliveryAddressesQueryHandler(
            IResult result,
            IRepositoryManager repositoryManager,
            IMapper mapper)
        {
            _result = result;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(ViewDeliveryAddressesQuery request, CancellationToken cancellationToken)
        {
            var deliveryAddresses = await _repositoryManager.DeliveryAddressRepo.AllAsync();
            if (deliveryAddresses.ToList().Count <= 0)
            {
                return _result.Failure(ResponseCodes.RecordNotFound, StatusCodes.Status404NotFound);
            }
            var nigerianStates = await _repositoryManager.NigerianStateRepo.AllAsync();

            var deliveryAddressesResponse = _mapper.Map<IList<DeliveryAddressResponse>>(deliveryAddresses);
            foreach (var deliveryAddress in deliveryAddressesResponse)
                deliveryAddress.State = nigerianStates.First(x => x.Id == deliveryAddress.Id).State;
            return _result.Success(deliveryAddressesResponse);
        }
    }
}

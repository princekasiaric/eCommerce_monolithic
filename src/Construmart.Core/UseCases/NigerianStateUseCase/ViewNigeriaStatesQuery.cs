using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Collections.Generic;

namespace Construmart.Core.UseCases.NigerianStateUseCase
{
    public class ViewNigerianStatesQuery : RequestContext<BaseResponse>
    {
        public ViewNigerianStatesQuery()
        {
        }
    }

    public class ViewNigeriaStatesQueryHandler : IRequestHandler<ViewNigerianStatesQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewNigeriaStatesQueryHandler(
            IResult result,
            IMapper mapper,
            IRepositoryManager repositoryManager)
        {
            _result = result;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(ViewNigerianStatesQuery request, CancellationToken cancellationToken)
        {
            var nigerianStates = await _repositoryManager.NigerianStateRepo.AllAsync();
            if (!nigerianStates.Any())
            {
                return _result.Failure(ResponseCodes.InvalidNigerianState, StatusCodes.Status404NotFound);
            }
            var nigerianStatesResponse = _mapper.Map<IEnumerable<NigerianStateResponse>>(nigerianStates);
            return _result.Success(nigerianStatesResponse);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.NigerianStateUseCase
{
    public class ViewNigerianStateQuery : RequestContext<BaseResponse>
    {
        public int NigerianStateId { get; private set; }

        public ViewNigerianStateQuery(int id)
        {
            NigerianStateId = id;
        }
    }

    public class ViewNigerianStateQueryValidator : AbstractValidator<ViewNigerianStateQuery>
    {
        public ViewNigerianStateQueryValidator()
        {
            RuleFor(x => x.NigerianStateId).GreaterThan(0);
        }
    }

    public class ViewNigerianStateQueryHandler : IRequestHandler<ViewNigerianStateQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewNigerianStateQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewNigerianStateQuery request, CancellationToken cancellationToken)
        {
            var nigerianState = await _repositoryManager.NigerianStateRepo.SingleOrDefaultAsync(x => x.Id == request.NigerianStateId);
            if (nigerianState == null)
            {
                return _result.Failure(ResponseCodes.InvalidNigerianState, StatusCodes.Status404NotFound);
            }
            var response = _mapper.Map<NigerianStateResponse>(nigerianState);
            return _result.Success(response);
        }
    }
}

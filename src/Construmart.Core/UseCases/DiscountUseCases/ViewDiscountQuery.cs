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

namespace Construmart.Core.UseCases.DiscountUseCases
{
    public class ViewDiscountQuery : RequestContext<BaseResponse>
    {
        public long DiscountId { get; private set; }

        public ViewDiscountQuery(long discountId)
        {
            DiscountId = discountId;
        }
    }

    public class ViewDiscountQueryHandler : IRequestHandler<ViewDiscountQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewDiscountQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
        {
            _result = result;
            _mapper = mapper;
            _repositoryManager = repositoryManager;
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(ViewDiscountQuery request, CancellationToken cancellationToken)
        {
            var discount = await _repositoryManager.DiscountRepo.SingleOrDefaultAsync(x => x.Id == request.DiscountId);
            if (discount == null)
            {
                return _result.Failure(ResponseCodes.InvalidDiscount, StatusCodes.Status404NotFound);
            }
            var discountResponse = _mapper.Map<DiscountResponse>(discount);
            return _result.Success(discountResponse);
        }
    }
}
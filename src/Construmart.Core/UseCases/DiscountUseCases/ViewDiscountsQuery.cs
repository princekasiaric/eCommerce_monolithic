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

namespace Construmart.Core.UseCases.DiscountUseCases
{
    public class ViewDiscountsQuery : RequestContext<BaseResponse>
    {
        public ViewDiscountsQuery()
        {

        }
    }

    public class ViewDiscountsQueryHandler : IRequestHandler<ViewDiscountsQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewDiscountsQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewDiscountsQuery request, CancellationToken cancellationToken)
        {
            var discounts = await _repositoryManager.DiscountRepo.AllAsync();
            var discountResponse = _mapper.Map<IList<DiscountResponse>>(discounts);
            return _result.Success(discountResponse);
        }
    }
}
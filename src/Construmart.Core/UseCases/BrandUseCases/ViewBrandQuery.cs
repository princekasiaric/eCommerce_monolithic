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

namespace Construmart.Core.UseCases.BrandUseCases
{
    public class ViewBrandQuery : RequestContext<BaseResponse>
    {
        public long BrandId { get; private set; }

        public ViewBrandQuery(long brandId)
        {
            BrandId = brandId;
        }
    }

    public class ViewBrandQueryHandler : IRequestHandler<ViewBrandQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewBrandQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewBrandQuery request, CancellationToken cancellationToken)
        {
            var brand = await _repositoryManager.BrandRepo.SingleOrDefaultAsync(x => x.Id == request.BrandId);
            if (brand == null)
            {
                return _result.Failure(ResponseCodes.InvalidBrand, StatusCodes.Status404NotFound);
            }
            var brandResponse = _mapper.Map<BrandResponse>(brand);
            return _result.Success(brandResponse);
        }
    }
}
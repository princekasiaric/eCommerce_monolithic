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
    public class ViewBrandsQuery : RequestContext<BaseResponse>
    {
        public ViewBrandsQuery()
        {

        }
    }

    public class ViewBrandsQueryHandler : IRequestHandler<ViewBrandsQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewBrandsQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _repositoryManager.BrandRepo.AllAsync();
            var brandResponse = _mapper.Map<IList<BrandResponse>>(brands);
            return _result.Success(brandResponse);
        }
    }
}
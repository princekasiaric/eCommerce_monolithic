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

namespace Construmart.Core.UseCases.TagUseCases
{
    public class ViewTagsQuery : RequestContext<BaseResponse>
    {
        public ViewTagsQuery()
        {

        }
    }

    public class ViewBrandsQueryHandler : IRequestHandler<ViewTagsQuery, BaseResponse>, IDisposable
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

        public async Task<BaseResponse> Handle(ViewTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repositoryManager.TagRepo.AllAsync();
            var tagResponse = _mapper.Map<IList<TagResponse>>(tags);
            return _result.Success(tagResponse);
        }
    }
}
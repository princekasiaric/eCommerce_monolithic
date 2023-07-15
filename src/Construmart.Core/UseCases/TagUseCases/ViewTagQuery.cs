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
    public class ViewTagQuery : RequestContext<BaseResponse>
    {
        public long TagId { get; private set; }

        public ViewTagQuery(long tagId)
        {
            TagId = tagId;
        }
    }

    public class ViewTagQueryHandler : IRequestHandler<ViewTagQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewTagQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewTagQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repositoryManager.TagRepo.SingleOrDefaultAsync(x => x.Id == request.TagId);
            if (tag == null)
            {
                return _result.Failure(ResponseCodes.InvalidTag, StatusCodes.Status404NotFound);
            }
            var tagResponse = _mapper.Map<TagResponse>(tag);
            return _result.Success(tagResponse);
        }
    }
}
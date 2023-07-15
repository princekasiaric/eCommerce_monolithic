using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CategoryUseCases
{
    public class ViewCategoryQuery : RequestContext<BaseResponse>
    {
        public long CategoryId { get; private set; }

        public ViewCategoryQuery(long categoryId)
        {
            CategoryId = categoryId;
        }
    }

    public class ViewCategoryQueryHandler : IRequestHandler<ViewCategoryQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewCategoryQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _repositoryManager.CategoryRepo.SingleOrDefaultAsync(x => x.Id == request.CategoryId);
            if (category == null)
            {
                return _result.Failure(ResponseCodes.InvalidCategory, StatusCodes.Status404NotFound);
            }
            var categoryResponse = _mapper.Map<CategoryResponse>(category);
            return _result.Success(categoryResponse);
        }
    }
}
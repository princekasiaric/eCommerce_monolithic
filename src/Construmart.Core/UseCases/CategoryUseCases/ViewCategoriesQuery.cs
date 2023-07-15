using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using MediatR;

namespace Construmart.Core.UseCases.CategoryUseCases
{
    public class ViewCategoriesQuery : RequestContext<BaseResponse>
    {
        public bool? IsActive { get; private set; }
        public bool? IsParent { get; private set; }
        public string SearchTerm { get; private set; }
        public ViewCategoriesQuery(FilterCategoriesParam request)
        {
            IsActive = request.IsActive;
            IsParent = request.IsParent;
            SearchTerm = request.SearchTerm;
        }
    }

    public class ViewCategoriesQueryHandler : IRequestHandler<ViewCategoriesQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewCategoriesQueryHandler(IResult result, IMapper mapper, IRepositoryManager repositoryManager)
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

        public async Task<BaseResponse> Handle(ViewCategoriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Category> categories = null;
            if (request.IsActive.HasValue && request.IsParent.HasValue)
            {
                categories = await _repositoryManager.CategoryRepo.WhereAsync(x => x.IsActive == request.IsActive.Value && x.IsParent == request.IsParent.Value);
            }
            else if (request.IsActive.HasValue && !request.IsParent.HasValue)
            {
                categories = await _repositoryManager.CategoryRepo.WhereAsync(x => x.IsActive == request.IsActive.Value);
            }
            else if (!request.IsActive.HasValue && request.IsParent.HasValue)
            {
                categories = await _repositoryManager.CategoryRepo.WhereAsync(x => x.IsParent == request.IsParent);
            }
            else
            {
                categories = await _repositoryManager.CategoryRepo.AllAsync();
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                categories = categories.Where(x => x.Name.ToLower().Contains(request.SearchTerm.ToLower()));
            }

            var response = _mapper.Map<List<CategoryResponse>>(categories);
            return _result.Success(response);
        }
    }
}
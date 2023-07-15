using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CategoryUseCases
{
    public class ViewProductsByCategoryIdQuery : RequestContext<BaseResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; private set; }
        public long CategoryId { get; private set; }

        public ViewProductsByCategoryIdQuery(long categoryId, CategoryParam queryString)
        {
            PageNumber = queryString.PageNumber;
            PageSize = queryString.PageSize;
            CategoryId = categoryId;
        }
    }

    public class ViewProductsByCategoryIdQueryHandler : IRequestHandler<ViewProductsByCategoryIdQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewProductsByCategoryIdQueryHandler(
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
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(ViewProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            var products = (await _repositoryManager.ProductRepo.PaginateAsync(request.PageNumber, request.PageSize,
                includes: new Expression<Func<Product, object>>[] { x => x.ProductImage },
                orderBy: x => x.DateCreated,
                isOrderAscending: false))
                .Where(x => x.ProductCategoryIds.Contains<long>(request.CategoryId));
            if (!products.Any())
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }
            var response = _mapper.Map<List<ProductResponse>>(products);
            return _result.Success(response);
        }
    }
}

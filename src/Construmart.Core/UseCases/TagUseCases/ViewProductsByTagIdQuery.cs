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

namespace Construmart.Core.UseCases.TagUseCases
{
    public class ViewProductsByTagIdQuery : RequestContext<BaseResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; private set; }
        public long TagId { get; private set; }

        public ViewProductsByTagIdQuery(long tagId, TagParam queryString)
        {
            PageNumber = queryString.PageNumber;
            PageSize = queryString.PageSize;
            TagId = tagId;
        }
    }

    public class ViewProductsByTagIdQueryHandler : IRequestHandler<ViewProductsByTagIdQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewProductsByTagIdQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewProductsByTagIdQuery request, CancellationToken cancellationToken)
        {
            var products = (await _repositoryManager.ProductRepo.PaginateAsync(request.PageNumber, request.PageSize,
                includes: new Expression<Func<Product, object>>[] { x => x.ProductImage },
                orderBy: x => x.DateCreated,
                isOrderAscending: false))
                .Where(x => x.ProductTagIds.Contains<long>(request.TagId));
            if (!products.Any())
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }
            var response = _mapper.Map<List<ProductResponse>>(products);
            return _result.Success(response);
        }
    }
}

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

namespace Construmart.Core.UseCases.ProductUseCases
{
    public class ViewProductsQuery : RequestContext<BaseResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; private set; }
        public bool? IsActive { get; private set; }
        public string SearchTerm { get; private set; }

        public ViewProductsQuery(FilterProductsParam request)
        {
            PageNumber = request.PageNumber;
            PageSize = request.PageSize;
            IsActive = request.IsActive;
            SearchTerm = request.SearchTerm;
        }
    }

    public class ViewProductsQueryHandler : IRequestHandler<ViewProductsQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewProductsQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewProductsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = null;
            if (request.IsActive.HasValue)
            {
                products = await _repositoryManager.ProductRepo.PaginateAsync(
                    request.PageNumber, request.PageSize, x => x.IsActive == request.IsActive,
                    includes: new Expression<Func<Product, object>>[] { x => x.ProductImage },
                    orderBy: x => x.DateCreated, isOrderAscending: false);
            }
            else
            {
                products = await _repositoryManager.ProductRepo.PaginateAsync(
                    request.PageNumber,
                    request.PageSize,
                    includes: new Expression<Func<Product, object>>[] { x => x.ProductImage },
                    orderBy: x => x.DateCreated, isOrderAscending: false);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                products = products.Where(x => x.Name.ToLower().Contains(request.SearchTerm.Trim().ToLower()));
            }
            var productCategories = new List<ProductCategoryResponse>();
            var productTags = new List<ProductTagResponse>();
            var response = new List<ProductResponse>();
            foreach (var product in products)
            {
                foreach (var id in product.ProductCategoryIds)
                {
                    var category = await _repositoryManager.CategoryRepo.SingleOrDefaultAsync(x => x.Id == id);
                    if (category != null)
                    {
                        productCategories.Add(new ProductCategoryResponse
                        {
                            Id = category.Id,
                            Name = category.Name
                        });
                    }
                    else
                    {
                        return _result.Failure(ResponseCodes.InvalidCategory, StatusCodes.Status404NotFound);
                    }
                }

                foreach (var id in product.ProductTagIds)
                {
                    var tag = await _repositoryManager.TagRepo.SingleOrDefaultAsync(x => x.Id == id);
                    if (tag != null)
                    {
                        productTags.Add(new ProductTagResponse
                        {
                            Id = tag.Id,
                            Name = tag.Name
                        });
                    }
                    else
                    {
                        return _result.Failure(ResponseCodes.InvalidTag, StatusCodes.Status404NotFound);
                    }
                }
                var productResponse = _mapper.Map<ProductResponse>(product);
                productResponse.ProductCategories = productCategories;
                productResponse.ProductTags = productTags;
                response.Add(productResponse);
            }
            return _result.Success(response);
        }
    }
}

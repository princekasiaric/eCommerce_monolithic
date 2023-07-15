using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.ProductUseCases
{
    public class ViewProductQuery : RequestContext<BaseResponse>
    {
        public long ProductId { get; private set; }

        public ViewProductQuery(long productId)
        {
            ProductId = productId;
        }
    }

    public class ViewProductQueryHandler : IRequestHandler<ViewProductQuery, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;

        public ViewProductQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _repositoryManager.ProductRepo.SingleOrDefaultAsync(
                x => x.Id == request.ProductId,
                includes: new Expression<Func<Product, object>>[] { x => x.ProductImage });
            if (product == null)
            {
                return _result.Failure(ResponseCodes.InvalidProduct, StatusCodes.Status404NotFound);
            }
            var productCategories = new List<ProductCategoryResponse>();
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
            var productTags = new List<ProductTagResponse>();
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
            return _result.Success(productResponse);
        }
    }
}

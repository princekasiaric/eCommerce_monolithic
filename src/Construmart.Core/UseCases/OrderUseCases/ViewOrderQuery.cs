using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper; 
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.OrderUseCases
{
    public class ViewOrderQuery : RequestContext<BaseResponse>
    {
        public long OrderId { get; private  set; }

        public ViewOrderQuery(long orderId)
        {
            OrderId = orderId;
        }
    }

    public class ViewOrderQueryHandler : IRequestHandler<ViewOrderQuery, BaseResponse>, IDisposable
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IResult _result;

        public ViewOrderQueryHandler(
                IRepositoryManager repositoryManager,
                IMapper mapper,
                IResult result)
        {
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _mapper = Guard.Against.Null(mapper, nameof(mapper));
            _result = Guard.Against.Null(result, nameof(result));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(ViewOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.OrderRepo.SingleOrDefaultAsync(x => x.Id == request.OrderId,
                includes: new Expression<Func<Domain.Models.OrderAggregate.Order, object>>[] { x => x.OrderItems });
            if (order == null)
            {
                return _result.Failure(ResponseCodes.RecordNotFound, StatusCodes.Status404NotFound);
            }

            var orderResponse = _mapper.Map<OrderResponse>(order);
            return _result.Success(orderResponse);
        }
    }
}

using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Request;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using MediatR;
using Construmart.Core.Domain.Models.OrderAggregate;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Construmart.Core.UseCases.OrderUseCases
{
    public class ViewOrdersQuery : RequestContext<BaseResponse>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; private set; }
        public string TrackingNumber { get; private set; }
        public string SearchTerm { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public ViewOrdersQuery(FilterOrdersParam param)
        {
            PageNumber = param.PageNumber;
            PageSize = param.PageSize;
            TrackingNumber = param.TrackingNumber;
            SearchTerm = param.SearchTerm;
            StartDate = param.StartDate;
            EndDate = param.EndDate;
        }
    }

    public class ViewOrdersQueryHandler : IRequestHandler<ViewOrdersQuery, BaseResponse>, IDisposable
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IResult _result;

        public ViewOrdersQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewOrdersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Order> orders;
            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                orders = await _repositoryManager.OrderRepo.PaginateAsync(
                    request.PageNumber,
                    request.PageSize,
                    x => x.DateCreated == request.StartDate && x.DateCreated == request.EndDate,
                    includes: new Expression<Func<Order, object>>[] { x => x.OrderItems },
                    orderBy: x => x.DateCreated,
                    isOrderAscending: false);
            }
            else if (request.TrackingNumber != null)
            {
                orders = await _repositoryManager.OrderRepo.PaginateAsync(
                    request.PageNumber,
                    request.PageSize,
                    x => x.TrackingNumber == request.TrackingNumber,
                    includes: new Expression<Func<Order, object>>[] { x => x.OrderItems },
                    orderBy: x => x.DateCreated,
                    isOrderAscending: false);
            }
            else
            {
                orders = await _repositoryManager.OrderRepo.PaginateAsync(
                    request.PageNumber,
                    request.PageSize,
                    includes: new Expression<Func<Order, object>>[] { x => x.OrderItems },
                    orderBy: x => x.DateCreated,
                    isOrderAscending: false);
            }

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                orders = orders.Where(x => x.TrackingNumber.ToLower().Contains(request.SearchTerm.Trim())
                || x.DateCreated == request.StartDate && x.DateCreated == request.EndDate);
            }

            var orderResponse = new List<OrderResponse>(orders.Count());
            foreach (var order in orders)
            {
                var response = _mapper.Map<OrderResponse>(order);
                orderResponse.Add(response);
            }
            return _result.Success(orderResponse);
        }
    }
}

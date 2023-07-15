using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
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
    public class ViewOrderByTrackingNumberQuery : RequestContext<BaseResponse>
    {
        public string TrackingNumber { get; private set; }

        public ViewOrderByTrackingNumberQuery(string trackingNuber)
        {
            TrackingNumber = trackingNuber;
        }
    }

    public class ViewOrderByTrackingNumberQueryValidator : AbstractValidator<ViewOrderByTrackingNumberQuery>
    {
        public ViewOrderByTrackingNumberQueryValidator()
        {
            RuleFor(x => x.TrackingNumber)
                .NotEmpty()
                .Matches(new Regex(Constants.AppRegex.ALPHANUMERIC))
                .WithMessage("Tracking number must be alphanumeric");
        }
    }

    public class ViewOrderByTrackingNumberQueryHandler : IRequestHandler<ViewOrderByTrackingNumberQuery, BaseResponse>, IDisposable
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IResult _result;

        public ViewOrderByTrackingNumberQueryHandler(
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

        public async Task<BaseResponse> Handle(ViewOrderByTrackingNumberQuery request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.OrderRepo.SingleOrDefaultAsync(x => x.TrackingNumber == request.TrackingNumber,
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

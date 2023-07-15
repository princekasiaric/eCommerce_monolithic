using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.OrderUseCases
{
    public class DeleteOrderCommand : RequestContext<BaseResponse>
    {
        public long OrderId { get; private set; }

        public DeleteOrderCommand(long orderId)
        {
            OrderId = orderId;
        }
    }

    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
        }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public DeleteOrderCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
        }

        public async Task<BaseResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repositoryManager.OrderRepo.SingleOrDefaultAsync(x => x.Id == request.OrderId,
                includes: new Expression<Func<Domain.Models.OrderAggregate.Order, object>>[] { x => x.OrderItems });
            if (order == null)
            {
                return _result.Failure(ResponseCodes.RecordNotFound, StatusCodes.Status404NotFound);
            }
            await _repositoryManager.OrderRepo.RemoveAsync(x => x.Id == order.Id);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}

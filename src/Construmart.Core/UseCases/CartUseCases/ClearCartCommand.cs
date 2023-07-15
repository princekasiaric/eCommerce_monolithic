using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.DTOs.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CartUseCases
{
    public class ClearCartCommand : RequestContext<BaseResponse>
    {
        public long CartId { get; private set; }

        public ClearCartCommand(long id)
        {
            CartId = id;
        }
    }

    public class ClearCartCommandHandler : IRequestHandler<ClearCartCommand, BaseResponse>, IDisposable
    {
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;

        public ClearCartCommandHandler(
            IResult result,
            IRepositoryManager repositoryManager)
        {
            _result = Guard.Against.Null(result, nameof(result));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<BaseResponse> Handle(ClearCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repositoryManager.CartRepo.SingleOrDefaultAsync(x => x.Id == request.CartId);
            if (cart == null)
            {
                return _result.Failure(ResponseCodes.InvalidCart, StatusCodes.Status404NotFound);
            }
            await _repositoryManager.CartRepo.RemoveAsync(x => x.Id == request.CartId);
            await _repositoryManager.SaveAsync();
            return _result.Success();
        }
    }
}

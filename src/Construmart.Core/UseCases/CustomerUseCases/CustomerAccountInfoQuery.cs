using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using Construmart.Core.Commons;
using Construmart.Core.DataContracts.Repositories;
using Construmart.Core.Domain.Models;
using Construmart.Core.DTOs.Response;
using Construmart.Core.ProcessorContracts.Identity;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Construmart.Core.UseCases.CustomerUseCases
{
    /// <summary>
    /// Query
    /// </summary>
    public class CustomerAccountInfoQuery : RequestContext<BaseResponse>
    {
        public CustomerAccountInfoQuery(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsPrincipal = claimsPrincipal;
        }

        public ClaimsPrincipal ClaimsPrincipal { get; }
    }

    /// <summary>
    /// Query handler
    /// </summary>
    public class CustomerAccountInfoQueryHandler : IRequestHandler<CustomerAccountInfoQuery, BaseResponse>, IDisposable
    {
        private readonly IMapper _objectMapper;
        private readonly IResult _result;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IIdentityService _identityService;

        public CustomerAccountInfoQueryHandler(
            IMapper objectMapper,
            IResult responseUtility,
            IRepositoryManager repositoryManager,
            IIdentityService identityService)
        {
            _objectMapper = Guard.Against.Null(objectMapper, nameof(objectMapper));
            _result = Guard.Against.Null(responseUtility, nameof(responseUtility));
            _repositoryManager = Guard.Against.Null(repositoryManager, nameof(repositoryManager));
            _identityService = Guard.Against.Null(identityService, nameof(identityService));
        }

        public void Dispose()
        {
            _repositoryManager.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<BaseResponse> Handle(CustomerAccountInfoQuery request, CancellationToken cancellationToken)
        {
            var userIdResult = _identityService.GetUserIdFromClaims(request.ClaimsPrincipal);
            if (userIdResult.IsSuccess == false)
            {
                return userIdResult;
            }
            var newResult = userIdResult as ServiceResponse<UserIdResponse>;
            var customer = await _repositoryManager.CustomerRepo.SingleOrDefaultAsync(x => x.ApplicationUserId == newResult.Payload.ApplicationUserId);
            if (customer == null)
            {
                return _result.Failure(ResponseCodes.InvalidUserAccount);
            }
            var payload = _objectMapper.Map<Customer, CustomerAccountInfoResponse>(customer);
            return _result.Success(payload);
        }
    }
}
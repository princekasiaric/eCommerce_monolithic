using System.Threading.Tasks;
using Construmart.Core.Commons;
using Construmart.Core.DTOs.Request;
using Construmart.Core.ProcessorContracts.Cache;
using Construmart.Core.ProcessorContracts.Identity.DTOs;
using Microsoft.Extensions.Logging;

namespace Construmart.Infrastructure.Processors.Cache
{
    public class CustomerCacheService
    {
        private readonly ILogger<CustomerCacheService> _logger;
        private readonly ICacheService _cacheService;
        public CustomerCacheService(ILogger<CustomerCacheService> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task SaveUserSignupRequest(CreateUserRequest request)
        {
            _logger.LogInformation("saving customer signup request");
            await _cacheService.SaveDataAsync(Constants.CacheKeys.CUSTOMER_SIGNUP + request.Email, request);
        }

        public async Task<CreateUserRequest> GetUserSignupRequest(string email) => await _cacheService.FetchDataAsync<CreateUserRequest>(Constants.CacheKeys.CUSTOMER_SIGNUP + email);
    }
}
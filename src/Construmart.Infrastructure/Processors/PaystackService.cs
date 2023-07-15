using System;
using System.Net.Http;
using System.Threading.Tasks;
using Construmart.Core.Commons;
using Construmart.Core.Configurations;
using Construmart.Core.ProcessorContracts.Paystack;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;

namespace Construmart.Infrastructure.Processors
{
    public class PaystackService : IVerifyTransactionService
    {
        private readonly IOptions<PayStackConfig> _appSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PaystackService(IOptions<PayStackConfig> appSettings, IHttpClientFactory httpClientFactory)
        {
            _appSettings = appSettings;
            _httpClientFactory = httpClientFactory;
            _httpClient = _httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(_appSettings.Value.BaseUrl);
        }

        public async Task<(bool isSuccess, string jsonResponse)> VerifyTransaction(string paymentReference)
        {
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(HeaderNames.Authorization, $"Bearer {Env.PayStackSecret}");
            var apiResponse = await _httpClient.GetAsync(_appSettings.Value.TransactionVerification + paymentReference);
            var responseContent = await apiResponse.Content.ReadAsStringAsync();
            if (apiResponse.IsSuccessStatusCode)
                return (true, responseContent);
            return (false, responseContent);
        }
    }
}

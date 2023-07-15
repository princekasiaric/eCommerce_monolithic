using System.Text.Json.Serialization;

namespace Construmart.Core.ProcessorContracts.Paystack
{
    public class DTOs
    {
        public class TransactionVerificationResponse
        {
            [JsonPropertyName("status")]
            public bool Status { get; set; }
            [JsonPropertyName("message")]
            public string Message { get; set; }
        }
    }
}

using System.Threading.Tasks;

namespace Construmart.Core.ProcessorContracts.Paystack
{
    public interface IVerifyTransactionService
    {
        Task<(bool isSuccess, string jsonResponse)> VerifyTransaction(string paymentReference);
    }
}

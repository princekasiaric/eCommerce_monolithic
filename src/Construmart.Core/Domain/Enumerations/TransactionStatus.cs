using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class TransactionStatus : EnumerationBase
    {
        public static readonly TransactionStatus Success = new TransactionStatus(1, nameof(Success));

        protected TransactionStatus()
        {
        }

        protected TransactionStatus(int value, string displayName) : base(value, displayName)
        {

        }
    }
}

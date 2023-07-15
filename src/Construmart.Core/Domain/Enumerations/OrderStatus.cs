using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class OrderStatus : EnumerationBase
    {
        public static readonly OrderStatus Initiated = new(1, nameof(Initiated));

        public OrderStatus()
        {
        }

        public OrderStatus(int value, string displayName) : base(value, displayName)
        {

        }
    }
}

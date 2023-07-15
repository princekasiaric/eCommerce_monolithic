using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class CurrencyCodes : EnumerationBase
    {
        public static readonly CurrencyCodes NGN = new CurrencyCodes(1, nameof(NGN));

        protected CurrencyCodes()
        {
        }

        protected CurrencyCodes(int value, string displayName) : base(value, displayName)
        {

        }
    }
}

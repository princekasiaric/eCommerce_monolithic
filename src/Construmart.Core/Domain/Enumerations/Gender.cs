using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class Gender : EnumerationBase
    {
        public static readonly Gender Male = new(1, nameof(Male));
        public static readonly Gender Female = new(2, nameof(Female));

        protected Gender()
        {
        }

        protected Gender(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
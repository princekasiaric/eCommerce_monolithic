using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class CustomerOnboardingStatus : EnumerationBase
    {
        public static readonly CustomerOnboardingStatus Initiated = new(1, nameof(Initiated));
        public static readonly CustomerOnboardingStatus Completed = new(2, nameof(Completed));

        protected CustomerOnboardingStatus()
        {
        }

        protected CustomerOnboardingStatus(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
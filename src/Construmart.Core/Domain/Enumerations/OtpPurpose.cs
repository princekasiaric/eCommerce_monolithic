using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class OtpPurpose : EnumerationBase
    {
        public static readonly OtpPurpose Signup = new(1, nameof(Signup));
        public static readonly OtpPurpose PasswordReset = new(2, nameof(PasswordReset));

        protected OtpPurpose()
        {
        }

        protected OtpPurpose(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
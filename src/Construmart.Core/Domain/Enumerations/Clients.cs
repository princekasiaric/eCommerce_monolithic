using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class Clients : EnumerationBase
    {
        public static readonly Clients AdministratorApp = new(1, nameof(AdministratorApp));
        public static readonly Clients CustomerApp = new(2, nameof(CustomerApp));
        public static readonly Clients DriverApp = new(3, nameof(DriverApp));

        protected Clients()
        {
        }

        protected Clients(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class RoleTypes : EnumerationBase
    {
        public static readonly RoleTypes SuperAdmin = new(1, nameof(SuperAdmin));
        public static readonly RoleTypes Admin = new(2, nameof(Admin));
        public static readonly RoleTypes Customer = new(3, nameof(Customer));
        public static readonly RoleTypes Driver = new(4, nameof(Driver));

        protected RoleTypes()
        {
        }

        protected RoleTypes(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
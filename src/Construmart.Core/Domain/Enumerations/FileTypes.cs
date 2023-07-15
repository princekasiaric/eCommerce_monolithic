using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construmart.Core.Domain.SeedWork;

namespace Construmart.Core.Domain.Enumerations
{
    public class FileTypes : EnumerationBase
    {
        public static readonly FileTypes Image = new(1, nameof(Image));
        public static readonly FileTypes Document = new(2, nameof(Document));
        public static readonly FileTypes ImageAndDocument = new(3, nameof(ImageAndDocument));

        protected FileTypes()
        {
        }

        protected FileTypes(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
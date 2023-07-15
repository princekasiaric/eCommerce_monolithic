using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Construmart.Core.Domain.Models
{
    public class ApplicationRole : IdentityRole<long>
    {
        public string Description { get; set; }
        public DateTime DateCreated { get; private set; }
        public DateTime? DateUpdated { get; private set; }
        public ICollection<IdentityUserRole<long>> UserRoles { get; private set; }
    }
}
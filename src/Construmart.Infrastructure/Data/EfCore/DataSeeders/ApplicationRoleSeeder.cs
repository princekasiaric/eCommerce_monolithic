using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.DataSeeders
{
    public static class ApplicationRoleSeeder
    {
        public static void SeedApplicationRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>(builder =>
                builder.HasData(new ApplicationRole
                {
                    Id = 1,
                    Name = RoleTypes.SuperAdmin.DisplayName,
                    NormalizedName = RoleTypes.SuperAdmin.DisplayName.ToUpper(),
                    Description = "Performs all administrative activities",
                })
            );
        }
    }
}
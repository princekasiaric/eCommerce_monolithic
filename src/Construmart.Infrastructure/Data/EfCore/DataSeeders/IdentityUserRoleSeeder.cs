using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.DataSeeders
{
    public static class IdentityUserRoleSeeder
    {
        public static void SeedIdentityUserRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<long>>(builder =>
                builder.HasData(new IdentityUserRole<long>
                {
                    RoleId = 1,
                    UserId = 1
                })
            );
        }
    }
}
using Construmart.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class ApplicationRoleConfig
    {
        public static void ConfigureApplicationRole(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationRole>(model =>
            {
                model.Property(t => t.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(t => t.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.HasMany(x => x.UserRoles).WithOne().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.NoAction);
                model.Metadata.FindNavigation(nameof(ApplicationUser.UserRoles)).SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }
    }
}
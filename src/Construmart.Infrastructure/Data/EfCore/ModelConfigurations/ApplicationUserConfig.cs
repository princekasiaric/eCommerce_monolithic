using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class ApplicationUserConfig
    {
        public static void ConfigureApplicationUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(model =>
            {
                model.HasIndex(x => x.UserName).IsUnique(false);
                model.OwnsOne(x => x.Otp, o =>
                {
                    o.Property(t => t.Purpose).HasConversion(x => x.Value, x => EnumerationBase.FromValue<OtpPurpose>(x));
                });
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.HasMany(x => x.UserRoles).WithOne().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.NoAction);
                model.Metadata.FindNavigation(nameof(ApplicationUser.UserRoles)).SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }
    }
}
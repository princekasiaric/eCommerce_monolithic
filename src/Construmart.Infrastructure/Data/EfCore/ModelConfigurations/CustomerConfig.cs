using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class CustomerConfig
    {
        public static void ConfigureCustomer(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.OwnsOne(x => x.Address);
                model.Property(x => x.Gender).HasConversion(x => x.Value, x => EnumerationBase.FromValue<Gender>(x));
                model.Property(x => x.OnboardingStatus).HasConversion(x => x.Value, x => EnumerationBase.FromValue<CustomerOnboardingStatus>(x));
            });
        }
    }
}
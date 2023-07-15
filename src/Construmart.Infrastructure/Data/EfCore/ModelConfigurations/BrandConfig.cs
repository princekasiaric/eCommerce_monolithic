using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class BrandConfig
    {
        public static void ConfigureBrand(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.HasIndex(x => x.Name);
            });
        }
    }
}

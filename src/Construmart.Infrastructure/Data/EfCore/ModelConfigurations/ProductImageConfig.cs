using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construmart.Core.Domain.Models.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class ProductImageConfig
    {
        public static void ConfigureProductImage(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductImage>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.OwnsOne(x => x.ImageFile, m =>
                {
                    m.HasIndex(x => x.Name);
                });
                model.HasOne<Product>().WithOne(x => x.ProductImage).HasForeignKey<ProductImage>(x => x.ProductId);
            });
        }
    }
}
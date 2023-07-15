using System;
using System.Collections.Generic;
using System.Text.Json;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class ProductConfig
    {
        public static void ConfigureProduct(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.HasIndex(x => x.Sku);
                model.HasIndex(x => x.Name);
                model.HasOne(x => x.ProductImage).WithOne().HasForeignKey<ProductImage>(x => x.ProductId);
                model.Property(x => x.ProductCategoryIds)
                    .HasConversion(v => JsonSerializer.Serialize(v, null), v => JsonSerializer.Deserialize<IList<long>>(v, null))
                    .HasField("_productCategoryIds")
                    .UsePropertyAccessMode(PropertyAccessMode.PreferField);
                model.Property(x => x.ProductTagIds)
                    .HasConversion(v => JsonSerializer.Serialize(v, null), v => JsonSerializer.Deserialize<IList<long>>(v, null))
                    .HasField("_productTagIds")
                    .UsePropertyAccessMode(PropertyAccessMode.PreferField);
                model.HasMany(x => x.ProductInventories).WithOne().HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
                model.Property(x => x.CurrencyCode).HasConversion(x => x.DisplayName, x => EnumerationBase.FromDisplayName<CurrencyCodes>(x, true));
                model.Metadata.FindNavigation(nameof(Product.ProductInventories)).SetPropertyAccessMode(PropertyAccessMode.Field);
            });
        }
    }
}

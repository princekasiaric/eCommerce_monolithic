using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models.OrderAggregate;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class OrderConfig
    {
        public static void ConfigureOrder(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated)
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated)
                    .ValueGeneratedOnUpdate()
                    .HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.HasIndex(x => x.TrackingNumber).IsUnique();
                model.HasMany(x => x.OrderItems)
                    .WithOne()
                    .HasForeignKey(x => x.OrderId).OnDelete(DeleteBehavior.Cascade);
                model.Property(x => x.OrderStatus).HasConversion(x => x.DisplayName, x => EnumerationBase.FromDisplayName<OrderStatus>(x, true));
            });
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Construmart.Core.Domain.Models.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class OrderItemConfig
    {
        public static void ConfigureOrderItem(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>(model =>
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
                model.HasOne<Order>()
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.OrderId);
            });
        }
    }
}
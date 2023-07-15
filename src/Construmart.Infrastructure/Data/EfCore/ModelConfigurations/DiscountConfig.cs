﻿using System;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class DiscountConfig
    {
        public static void ConfigureDiscount(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Discount>(model =>
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

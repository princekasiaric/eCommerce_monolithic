using System;
using System.Diagnostics.CodeAnalysis;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.Models.ProductAggregate;
using Construmart.Infrastructure.Data.EfCore.DataSeeders;
using Construmart.Infrastructure.Data.EfCore.ModelConfigurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore
{
    public class RepositoryContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public RepositoryContext()
        {

        }

        public RepositoryContext([NotNullAttribute] DbContextOptions<RepositoryContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //model configurations
            modelBuilder.ConfigureApplicationUser();
            modelBuilder.ConfigureApplicationRole();
            modelBuilder.ConfigureCustomer();
            modelBuilder.ConfigureCategory();
            modelBuilder.ConfigureBrand();
            modelBuilder.ConfigureTag();
            modelBuilder.ConfigureDiscount();
            modelBuilder.ConfigureProductImage();
            modelBuilder.ConfigureProductInventory();
            modelBuilder.ConfigureProduct();
            modelBuilder.ConfigureCart();
            modelBuilder.ConfigureCartItem();
            modelBuilder.ConfigureDeliveryAddress();
            modelBuilder.ConfigureNigerianState();
            modelBuilder.ConfigureOrder();
            modelBuilder.ConfigureOrderItem();
            modelBuilder.ConfigureTransaction();

            //data seeds
            modelBuilder.SeedApplicationRole();
            modelBuilder.SeedApplicationUser();
            modelBuilder.SeedIdentityUserRole();
            modelBuilder.SeedNigerianState();
        }
    }
}
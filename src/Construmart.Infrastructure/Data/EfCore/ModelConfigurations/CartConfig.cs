using Construmart.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class CartConfig
    {
        public static void ConfigureCart(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.HasMany(x => x.CartItems).WithOne().HasForeignKey(x => x.CartId).OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

using Construmart.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class CartItemConfig
    {
        public static void ConfigureCartItem(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.HasOne<Cart>().WithMany(x => x.CartItems).HasForeignKey(x => x.CartId);
            });
        }
    }
}

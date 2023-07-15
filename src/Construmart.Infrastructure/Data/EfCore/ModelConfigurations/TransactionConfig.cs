using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Construmart.Core.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class TransactionConfig
    {
        public static void ConfigureTransaction(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Property(x => x.DateCreated).ValueGeneratedOnAdd().HasDefaultValueSql("GETDATE()");
                model.Property(x => x.DateUpdated).ValueGeneratedOnUpdate().HasDefaultValueSql("GETDATE()");
                model.Ignore(x => x.DomainEvents);
                model.Property(x => x.TransactionStatus).HasConversion(x => x.DisplayName, x => EnumerationBase.FromDisplayName<TransactionStatus>(x, true));
            });
        }
    }
}

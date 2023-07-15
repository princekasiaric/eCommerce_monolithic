using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Construmart.Core.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.ModelConfigurations
{
    public static class NigerianStateConfig
    {
        public static void ConfigureNigerianState(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NigerianState>(model =>
            {
                model.HasKey(x => x.Id);
                model.Property(x => x.RowVersion).IsRowVersion();
                model.Ignore(x => x.DomainEvents);
                model.Property(x => x.LocalGovernmentAreas)
                    .HasConversion(v => JsonSerializer.Serialize(v, null), v => JsonSerializer.Deserialize<IList<string>>(v, null));
            });
        }
    }
}
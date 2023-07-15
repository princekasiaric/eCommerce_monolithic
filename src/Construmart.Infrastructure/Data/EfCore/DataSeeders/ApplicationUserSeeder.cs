using System;
using System.Collections.Generic;
using Construmart.Core.Domain.Enumerations;
using Construmart.Core.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Construmart.Infrastructure.Data.EfCore.DataSeeders
{
    public static class ApplicationUserSeeder
    {
        public static void SeedApplicationUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>(builder =>
            {
                var superAdmin = new ApplicationUser
                (
                    1,
                    "testadmin@email.com" + Clients.AdministratorApp.DisplayName,
                    ("testadmin@email.com" + Clients.AdministratorApp.DisplayName).ToUpper(),
                    "testadmin@email.com",
                    true,
                    false,
                    true,
                    securityStamp: Guid.NewGuid().ToString()
                );
                superAdmin.SavePassword("P@ssw0rd");
                builder.HasData(superAdmin);
            });
        }
    }
}
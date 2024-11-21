using app.Infrastructure.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Infrastructure
{
    public class FixedData
    {
        public static void SeedData(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                   new IdentityRole { Id = "8e445865-a24d-4543-a6c6-9443d048cdb9", Name = "Customer", NormalizedName = "CUSTOMER" },
            new IdentityRole { Id = "2c5e174e-3b0e-446f-86af-483d56fd7210", Name = "Admin", NormalizedName = "ADMIN" }
           );
            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "0f04028e-587c-47ad-8b36-6dbd6a059fa4",
                    PhoneNumber = "01840019826",
                    Email = "jishan.bd46@gmail.com",
                    EmailConfirmed = true,
                    FullName = "System Admin",
                    LockoutEnabled = false,
                    NormalizedEmail = "JISHAN.BD46@GMAIL.COM",
                    NormalizedUserName = "ADMINISTRATOR",
                    PasswordHash = "AQAAAAEAACcQAAAAEE8d8uAFK+zBNJ3j+s3k5c6D+OqrJJqgpV0CF42z2UDwqm/kSD/LWNXN8OAx/56YHg==",
                    ConcurrencyStamp = "616a2e8f-dc94-4576-8ec4-c9d75d1df6d1",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = "37QJAUUNCSSXNFFB6ZXI6OJLHSCS5J6I",
                    TwoFactorEnabled = false,
                    UserName = "administrator",
                    UserType = 1
                },
                new ApplicationUser
                {
                    Id = "0f04028e-587c-37ad-8b36-6dbd6a059fa10",
                    PhoneNumber = "01840019826",
                    Email = "cus.jishan@gmail.com",
                    EmailConfirmed = true,
                    FullName = "System Engineers",
                    LockoutEnabled = false,
                    NormalizedEmail = "CUS.JISHAN@GMAIL.COM",
                    NormalizedUserName = "CUSTOMER",
                    PasswordHash = "AQAAAAEAACcQAAAAEE8d8uAFK+zBNJ3j+s3k5c6D+OqrJJqgpV0CF42z2UDwqm/kSD/LWNXN8OAx/56YHg==",
                    ConcurrencyStamp = "616a2e8f-dc94-4576-8ec4-c9d75d1df6d9",
                    PhoneNumberConfirmed = true,
                    SecurityStamp = "37QJAUUNCSSXNFFB6ZXI6OJLHSCS5J63",
                    TwoFactorEnabled = false,
                    UserName = "Customer",
                    UserType = 2
                }
            );


            builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                UserId = "0f04028e-587c-47ad-8b36-6dbd6a059fa4"
            },

             //Admin
             new IdentityUserRole<string>
             {
                 RoleId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
                 UserId = "0f04028e-587c-37ad-8b36-6dbd6a059fa10"
             }
             
         );
        }


    }
}

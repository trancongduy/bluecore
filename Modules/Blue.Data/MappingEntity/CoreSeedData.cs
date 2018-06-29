using System;
using Microsoft.EntityFrameworkCore;
using Blue.Data.IdentityModel;
using Framework.Common.Helpers;

namespace Blue.Data.MappingEntity
{
    public static class CoreSeedData
    {
        public static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Id = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), Name = "admin", NormalizedName = "ADMIN", CreatedDate = DateTimeOffset.Now, CreatedBy = "systemadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "systemadmin" },
                new Role { Id = IdHelper.GenerateId("7a749297-4edf-4d16-a769-d3bada83247e"), Name = "customer", NormalizedName = "CUSTOMER", CreatedDate = DateTimeOffset.Now, CreatedBy = "systemadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "systemadmin" },
                new Role { Id = IdHelper.GenerateId("61f3dc6e-3863-40e5-ba2b-a6334b5590ac"), Name = "guest", NormalizedName = "GUEST", CreatedDate = DateTimeOffset.Now, CreatedBy = "systemadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "systemadmin" }
            );

            builder.Entity<User>().HasData(
                new User { IsDeleted = true, Id = IdHelper.GenerateId("53319368-f467-4d49-a1b0-8da303b6c24a"), FirstName = "System", LastName = "Administrator", UserName = "systemadmin", NormalizedUserName = "SYSTEMADMIN", Email = "system@gmail.com", NormalizedEmail = "SYSTEM@SIMPLCOMMERCE.COM", PasswordHash = "AQAAAAEAACcQAAAAEFB+UXfZPfZvMWdDDl9SSWPNDoKKBpBASb/CzK/rmWu/OlE15ALyEAY/QKP4jEkRyg==", SecurityStamp = Guid.NewGuid().ToString(), CreatedDate = DateTimeOffset.Now, CreatedBy = "systemadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "systemadmin" },
                new User { Id = IdHelper.GenerateId("55e816d6-34e1-4a1c-9940-4bc5381b21b8"), FirstName = "Shop", LastName = "Admin", UserName = "admin", NormalizedUserName = "ADMIN", Email = "admin@gmail.com", NormalizedEmail = "ADMIN@GMAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEFB+UXfZPfZvMWdDDl9SSWPNDoKKBpBASb/CzK/rmWu/OlE15ALyEAY/QKP4jEkRyg==", SecurityStamp = Guid.NewGuid().ToString(), CreatedDate = DateTimeOffset.Now, CreatedBy = "systemadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "systemadmin" }
            );

            builder.Entity<UserRole>().HasData(
                new UserRole { RoleId = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), UserId = IdHelper.GenerateId("53319368-f467-4d49-a1b0-8da303b6c24a") },
                new UserRole { RoleId = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), UserId = IdHelper.GenerateId("55e816d6-34e1-4a1c-9940-4bc5381b21b8") }
            );
        }
    }
}

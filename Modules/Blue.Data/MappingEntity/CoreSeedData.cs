using System;
using Microsoft.EntityFrameworkCore;
using Blue.Data.Models.IdentityModel;
using Framework.Common.Helpers;

namespace Blue.Data.MappingEntity
{
    public static class CoreSeedData
    {
        public static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
                new Role { Active = true, IsDeleted = true, Id = IdHelper.GenerateId("11111111-1111-1111-1111-111111111111"), Code = "SUPERADMIN", Name = "Super Adminitrator", NormalizedName = "SUPER ADMINISTRATOR", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), Code = "ADMIN", Name = "Admin", NormalizedName = "ADMINISTRATOR", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("7a749297-4edf-4d16-a769-d3bada83247e"), Code = "USER", Name = "User", NormalizedName = "USER", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("61f3dc6e-3863-40e5-ba2b-a6334b5590ac"), Code = "EMPMGR", Name = "Employ Manager", NormalizedName = "EMPLOY MANAGER", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("94efbd9d-49f3-48b6-ba6d-57ace0d753f4"), Code = "EMPSEC", Name = "Employ Secretary", NormalizedName = "EMPLOY SECRETARY", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" }
            );

            builder.Entity<User>().HasData(
                new User { Active = true, IsDeleted = true, Id = IdHelper.GenerateId("53319368-f467-4d49-a1b0-8da303b6c24a"), FirstName = "Super", LastName = "Administrator", UserName = "superadmin", NormalizedUserName = "SUPERADMIN", Email = "superadmin@gmail.com", NormalizedEmail = "SUPERADMIN@GMAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEFB+UXfZPfZvMWdDDl9SSWPNDoKKBpBASb/CzK/rmWu/OlE15ALyEAY/QKP4jEkRyg==", SecurityStamp = Guid.NewGuid().ToString(), CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new User { Active = true, Id = IdHelper.GenerateId("55e816d6-34e1-4a1c-9940-4bc5381b21b8"), FirstName = "Shop", LastName = "Administrator", UserName = "admin", NormalizedUserName = "ADMIN", Email = "admin@gmail.com", NormalizedEmail = "ADMIN@GMAIL.COM", PasswordHash = "AQAAAAEAACcQAAAAEFB+UXfZPfZvMWdDDl9SSWPNDoKKBpBASb/CzK/rmWu/OlE15ALyEAY/QKP4jEkRyg==", SecurityStamp = Guid.NewGuid().ToString(), CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" }
            );

            builder.Entity<UserRole>().HasData(
                new UserRole { RoleId = IdHelper.GenerateId("11111111-1111-1111-1111-111111111111"), UserId = IdHelper.GenerateId("53319368-f467-4d49-a1b0-8da303b6c24a") },
                new UserRole { RoleId = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), UserId = IdHelper.GenerateId("55e816d6-34e1-4a1c-9940-4bc5381b21b8") }
            );
        }
    }
}

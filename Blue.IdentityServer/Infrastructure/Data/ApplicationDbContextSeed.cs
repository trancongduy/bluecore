using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blue.Data.Models.IdentityModel;
using Framework.Common.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Blue.IdentityServer.Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public async Task SeedAsync(ApplicationDbContext context, IHostingEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = 0)
        {
            int retryForAvaiability = retry.Value;

            try
            {
                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(GetDefaultRole());

                    await context.SaveChangesAsync();
                }

                if (!context.Users.Any())
                {
                    var user1 =
                        new User
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Super",
                            LastName = "Administrator",
                            PhoneNumber = "1234567890",
                            UserName = "superadmin",
                            Email = "superadmin@gmail.com",
                            NormalizedUserName = "SUPERADMIN",
                            NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                            SecurityStamp = Guid.NewGuid().ToString("D"),
                            Active = true,
                            IsDeleted = true,
                            CreatedDate = DateTimeOffset.Now,
                            CreatedBy = "superadmin",
                            UpdatedDate = DateTimeOffset.Now,
                            UpdatedBy = "superadmin"
                        };

                    user1.PasswordHash = _passwordHasher.HashPassword(user1, "1qazZAQ!");

                    var user2 =
                        new User
                        {
                            Id = Guid.NewGuid(),
                            FirstName = "Shop",
                            LastName = "Administrator",
                            PhoneNumber = "1234567890",
                            UserName = "admin",
                            Email = "admin@gmail.com",
                            NormalizedUserName = "ADMIN",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            SecurityStamp = Guid.NewGuid().ToString("D"),
                            Active = true,
                            IsDeleted = false,
                            CreatedDate = DateTimeOffset.Now,
                            CreatedBy = "superadmin",
                            UpdatedDate = DateTimeOffset.Now,
                            UpdatedBy = "superadmin"
                        };

                    user2.PasswordHash = _passwordHasher.HashPassword(user2, "1qazZAQ!");

                    context.Users.AddRange(new List<User> {user1, user2});

                    await context.SaveChangesAsync();

                    var userRole1 = new UserRole
                    {
                        RoleId = IdHelper.GenerateId("11111111-1111-1111-1111-111111111111"),
                        UserId = user1.Id
                    };

                    var userRole2 = new UserRole
                    {
                        RoleId = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"),
                        UserId = user2.Id
                    };

                    context.UserRoles.AddRange(new List<UserRole> {userRole1, userRole2});

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex.Message, $"There is an error migrating data for ApplicationDbContext");

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }

        private IEnumerable<Role> GetDefaultRole()
        {
            return new List<Role>
            {
                new Role { Active = true, IsDeleted = true, Id = IdHelper.GenerateId("11111111-1111-1111-1111-111111111111"), Code = "SUPERADMIN", Name = "Super Adminitrator", NormalizedName = "SUPER ADMINISTRATOR", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("5a61f008-0ce1-4b60-8c0c-c12c721e475d"), Code = "ADMIN", Name = "Admin", NormalizedName = "ADMINISTRATOR", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("7a749297-4edf-4d16-a769-d3bada83247e"), Code = "USER", Name = "User", NormalizedName = "USER", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("61f3dc6e-3863-40e5-ba2b-a6334b5590ac"), Code = "EMPMGR", Name = "Employ Manager", NormalizedName = "EMPLOY MANAGER", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" },
                new Role { Active = true, Id = IdHelper.GenerateId("94efbd9d-49f3-48b6-ba6d-57ace0d753f4"), Code = "EMPSEC", Name = "Employ Secretary", NormalizedName = "EMPLOY SECRETARY", CreatedDate = DateTimeOffset.Now, CreatedBy = "superadmin", UpdatedDate = DateTimeOffset.Now, UpdatedBy = "superadmin" }
            };
        }
    }
}

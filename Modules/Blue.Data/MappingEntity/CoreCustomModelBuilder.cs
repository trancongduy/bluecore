using Microsoft.EntityFrameworkCore;
using Blue.Data.Models.IdentityModel;
using Framework.Data.Interfaces;

namespace Blue.Data.MappingEntity
{
    public class CoreCustomModelBuilder : ICustomModelBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
               .ToTable("AspNetUsers");

            modelBuilder.Entity<Role>()
                .ToTable("AspNetRoles");

            modelBuilder.Entity<UserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.HasOne(uc => uc.User).WithMany(x => x.UserClaims).HasForeignKey(u => u.UserId);
                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity<RoleClaim>(b =>
            {
                b.HasKey(rc => rc.Id);
                b.HasOne(rc => rc.Role).WithMany(x => x.RoleClaims).HasForeignKey(r => r.RoleId);
                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => new {ur.UserId, ur.RoleId});
                b.HasOne(ur => ur.Role).WithMany(x => x.Users).HasForeignKey(r => r.RoleId);
                b.HasOne(ur => ur.User).WithMany(x => x.Roles).HasForeignKey(u => u.UserId);
                b.ToTable("AspNetUserRoles");
            });

            modelBuilder.Entity<UserLogin>(b =>
            {
                b.HasKey(ul => new {ul.UserId, ul.LoginProvider});
                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity<UserToken>(b =>
            {
                b.HasKey(ut => new {ut.UserId, ut.LoginProvider, ut.Name});
                b.ToTable("AspNetUserTokens");
            });


            CoreSeedData.SeedData(modelBuilder);
        }
    }
}

using Autofac;
using Blue.Data.IdentityModel;
using Blue.Data.IdentityService;
using Microsoft.AspNetCore.Identity;

namespace Blue.Api.Extensions
{
    public class IdentityConfigModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserStore>().As<IUserStore<User>>().InstancePerLifetimeScope();
            builder.RegisterType<RoleStore>().As<IRoleStore<Role>>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerLifetimeScope();
        }
    }
}

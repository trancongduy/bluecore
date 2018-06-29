using Autofac;
using Blue.Data.IdentityService;
using Blue.Data.Models.IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Blue.Api.Extensions
{
    public class IdentityConfigModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserStore>().As<IUserStore<User>>().InstancePerRequest();
            builder.RegisterType<RoleStore>().As<IRoleStore<Role>>().InstancePerRequest();
            builder.RegisterType<ApplicationUserManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationSignInManager>().AsSelf().InstancePerRequest();
            builder.RegisterType<ApplicationRoleManager>().AsSelf().InstancePerRequest();
        }
    }
}

using System.Collections.Generic;
using Blue.Data.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Blue.Data.IdentityService
{
    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        //public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        //{
        //    return new ApplicationRoleManager(new ApplicationRoleStore(context.Get<ApplicationDbContext>()));
        //}
    }
}

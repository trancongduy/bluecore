using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blue.Data.Models.IdentityModel
{
    public class UserStore : UserStore<User, Role, BlueDbContext, Guid, UserClaim, UserRole, UserLogin, UserToken, RoleClaim>
    {
        public UserStore(BlueDbContext context, IdentityErrorDescriber describer) : base(context, describer)
        {
        }
    }
}

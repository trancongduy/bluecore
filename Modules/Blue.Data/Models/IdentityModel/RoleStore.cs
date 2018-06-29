using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Blue.Data.Models.IdentityModel
{
    public class RoleStore : RoleStore<Role, BlueDbContext, Guid, UserRole, RoleClaim>
    {
		public RoleStore(BlueDbContext context) : base(context)
        {
		}

        protected override RoleClaim CreateRoleClaim(Role role, Claim claim)
		{
            return new RoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
		}
    }
}

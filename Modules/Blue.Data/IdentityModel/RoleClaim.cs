using System;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.IdentityModel
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}

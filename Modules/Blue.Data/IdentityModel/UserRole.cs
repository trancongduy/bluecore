using System;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.IdentityModel
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public override Guid UserId { get; set; }

        public override Guid RoleId { get; set; }

        public virtual User User { get; set; }

        public virtual Role Role { get; set; }
    }
}

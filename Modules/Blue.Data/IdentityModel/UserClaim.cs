using System;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.IdentityModel
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}

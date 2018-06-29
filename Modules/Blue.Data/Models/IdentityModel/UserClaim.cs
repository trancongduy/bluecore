using System;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.Models.IdentityModel
{
    public class UserClaim : IdentityUserClaim<Guid>
    {
        public virtual User User { get; set; }
    }
}

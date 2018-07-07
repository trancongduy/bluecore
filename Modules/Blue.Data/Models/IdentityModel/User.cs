using System;
using System.Collections.Generic;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.Models.IdentityModel
{
    public class User : IdentityUser<Guid>, IAuditableEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

		public string Address { get; set; }

        public bool RememberMe { get; set; }

        public bool ShouldLockout { get; set; }

        public bool IsDeleted { get; set; }

        public bool Active { get; set; }

        public bool IsLocked { get; set; }

        public DateTimeOffset LockedDate { get; set; }

        public DateTimeOffset UnLockedDate { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual IList<UserRole> Roles { get; set; } = new List<UserRole>();

        public virtual IList<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
    }

}

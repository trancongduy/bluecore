using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using Framework.Constract.Interfaces;

namespace Blue.Data.IdentityModel
{
    public class Role : IdentityRole<Guid>, IAuditableEntity
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual IList<UserRole> Users { get; set; } = new List<UserRole>();

        public virtual IList<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}

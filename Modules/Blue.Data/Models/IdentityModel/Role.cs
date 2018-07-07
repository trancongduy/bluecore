using System;
using System.Collections.Generic;
using Framework.Constract.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Blue.Data.Models.IdentityModel
{
    public class Role : IdentityRole<Guid>, IAuditableEntity
    {
        public Role()
        {
        }

        public Role(string roleName) : base(roleName)
        {
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public bool Active { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public virtual IList<UserRole> Users { get; set; } = new List<UserRole>();

        public virtual IList<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}

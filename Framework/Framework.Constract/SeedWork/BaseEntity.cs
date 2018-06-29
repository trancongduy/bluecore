using System;
using System.ComponentModel.DataAnnotations;
using Framework.Constract.Interfaces;

namespace Framework.Constract.SeedWork
{
    public abstract class BaseEntity : IAuditableEntity, IIdentity
    {
        protected BaseEntity() : this(Guid.NewGuid())
        {
        }

        protected BaseEntity(Guid id)
        {
            Id = id;
        }

        [Key]
        public Guid Id { get; protected set; }

        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}

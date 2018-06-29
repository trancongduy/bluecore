using System;
using Framework.Constract.Interfaces;

namespace Framework.Constract.SeedWork
{
    public abstract class BaseDto : AuditableDto
    {
        public Guid Id { get; protected set; }
    }

    public abstract class AuditableDto : IAuditableDto
    {
        public DateTimeOffset CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTimeOffset UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
    }
}

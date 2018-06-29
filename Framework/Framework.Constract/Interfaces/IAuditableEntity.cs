using System;

namespace Framework.Constract.Interfaces
{
	public interface IAuditableEntity
	{
		DateTimeOffset CreatedDate { get; set; }

		string CreatedBy { get; set; }

	    DateTimeOffset UpdatedDate { get; set; }

		string UpdatedBy { get; set; }

        bool IsDeleted { get; set; }
	}
}

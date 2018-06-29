using System;

namespace Framework.Constract.Interfaces
{
	public interface IAuditableDto
	{
	    DateTimeOffset CreatedDate { get; set; }

		string CreatedBy { get; set; }

	    DateTimeOffset UpdatedDate { get; set; }

		string UpdatedBy { get; set; }
	}
}

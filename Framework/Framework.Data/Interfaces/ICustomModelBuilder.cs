using Microsoft.EntityFrameworkCore;

namespace Framework.Data.Interfaces
{
	public interface ICustomModelBuilder
	{
		void Build(ModelBuilder modelBuilder);
	}
}

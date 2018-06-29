using Blue.Data.Models;
using Framework.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blue.Data.MappingEntity
{
    //public class ContactConfiguration : ICustomModelBuilder
    //{
    //    public void Build(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<Contact>().ToTable("Contact");
    //    }
    //}

    public class ContactConfiguration : EntityMappingConfiguration<Contact>
    {
        public override void Map(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contact");
        }
    }
}

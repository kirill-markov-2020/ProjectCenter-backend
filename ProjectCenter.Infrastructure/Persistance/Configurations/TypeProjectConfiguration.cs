using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class TypeProjectConfiguration : IEntityTypeConfiguration<TypeProject>
    {
        public void Configure(EntityTypeBuilder<TypeProject> builder)
        {
            builder.ToTable("TypeProject");
            builder.Property(t => t.Name).HasMaxLength(50).IsRequired();
        }
    }
}

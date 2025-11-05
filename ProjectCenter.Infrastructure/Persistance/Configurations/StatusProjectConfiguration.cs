using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class StatusProjectConfiguration : IEntityTypeConfiguration<StatusProject>
    {
        public void Configure(EntityTypeBuilder<StatusProject> builder)
        {
            builder.ToTable("StatusProject");
            builder.Property(s => s.Name).HasMaxLength(50).IsRequired();
        }
    }
}

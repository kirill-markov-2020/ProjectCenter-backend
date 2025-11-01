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
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(50);
        }
    }
}

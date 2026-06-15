using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class DataStorageCategoryConfiguration : IEntityTypeConfiguration<DataStorageCategory>
    {
        public void Configure(EntityTypeBuilder<DataStorageCategory> builder)
        {
            builder.ToTable("DataStorageCategory");
            builder.Property(d => d.Name).HasMaxLength(200).IsRequired();
            builder.Property(d => d.Purpose).HasMaxLength(500);
            builder.Property(d => d.RetentionPeriod).HasMaxLength(200);
        }
    }
}

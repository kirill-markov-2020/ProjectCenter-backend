using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class TypeNotificationConfiguration : IEntityTypeConfiguration<TypeNotification>
    {
        public void Configure(EntityTypeBuilder<TypeNotification> builder)
        {
            builder.ToTable("TypeNotification");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(t => t.Name)
                .IsUnique()
                .HasDatabaseName("UX_TypeNotification_Name");
        }
    }
}
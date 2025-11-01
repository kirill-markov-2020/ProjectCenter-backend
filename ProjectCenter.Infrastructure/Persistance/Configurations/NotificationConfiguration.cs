using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectCenter.Core.Entities;

namespace ProjectCenter.Infrastructure.Persistence.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notification");
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Text).IsRequired().HasMaxLength(50);

            builder.HasOne(n => n.Sender)
                   .WithMany()
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Recipient)
                   .WithMany()
                   .HasForeignKey(n => n.RecipientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

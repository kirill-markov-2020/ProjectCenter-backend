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
            builder.Property(n => n.Text).HasMaxLength(1000).IsRequired();

            builder.HasOne(n => n.Sender)
                   .WithMany(u => u.SentNotifications)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Recipient)
                   .WithMany(u => u.ReceivedNotifications)
                   .HasForeignKey(n => n.RecipientId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
